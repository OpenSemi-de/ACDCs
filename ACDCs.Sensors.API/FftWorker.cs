// ReSharper disable MemberCanBePrivate.Global
namespace ACDCs.Sensors.API;

using System.Collections.Concurrent;

public class FftWorker
{
    private int _fftWindowSize = 256;
    private Filter _filter = Filter.None;
    private double _filterFrequency = 0;
    private bool _isRunning = true;
    private ConcurrentQueue<FftSample> _samples = new();
    private Thread _thread;

    public int FftWindowSize
    {
        get => _fftWindowSize;
        set
        {
            UseMutex(() => { _fftWindowSize = value; });
        }
    }

    public Filter Filter
    {
        get => _filter;
        set
        {
            UseMutex(() => _filter = value);
        }
    }

    public double FilterFrequency
    {
        get => _filterFrequency;
        set
        {
            UseMutex(() => _filterFrequency = value);
        }
    }

    public double FilterFrequencyMax { get; set; }

    public bool IsRunning
    {
        get => _isRunning;
        set
        {
            UseMutex(() => { _isRunning = value; });
        }
    }

    public Mutex Mutex { get; set; } = new();
    public ConcurrentQueue<FftInfoPacket> OutputQueue { get; } = new();

    public ConcurrentQueue<FftSample> Samples
    {
        get => _samples;
        set
        {
            UseMutex(() => { _samples = value; });
        }
    }

    public FftWorker()
    {
        _thread = GetThread();
    }

    public void EnqueueSample(FftSample fftSample)
    {
        _samples.Enqueue(fftSample);
    }

    private void Enqueue(FftInfoPacket seriesFft)
    {
        if (seriesFft.Count < 1) return;
        OutputQueue.Enqueue(seriesFft);
        Task.Run(() =>
        {
            while (_samples.Count > 5000) _samples.TryDequeue(out _);
        });
    }

    private async void Fft_BackgroundTask()
    {
        while (true)
        {
            if (IsRunning)
            {
                FftInfoPacket seriesFft = new();
                await GetFft(seriesFft);
                Enqueue(seriesFft);
            }
            while (OutputQueue.Count > 1)
                await Task.Delay(50);
        }
        // ReSharper disable once FunctionNeverReturns
    }

    private async Task GetFft(FftInfoPacket values)
    {
        double[] signal = await GetLastSamples();
        if (signal.Length < FftWindowSize)
        {
            return;
        }

        switch (Filter)
        {
            case Filter.LowPass:
                signal = FftSharp.Filter.LowPass(signal, 100, maxFrequency: FilterFrequency);
                break;

            case Filter.HighPass:
                signal = FftSharp.Filter.HighPass(signal, 100, minFrequency: FilterFrequency);
                break;

            case Filter.BandPass:
                signal = FftSharp.Filter.BandPass(signal, 100, minFrequency: FilterFrequency, maxFrequency: FilterFrequencyMax);
                break;

            case Filter.BandStop:
                signal = FftSharp.Filter.BandStop(signal, 100, minFrequency: FilterFrequency, maxFrequency: FilterFrequencyMax);
                break;
        }

        const int sampleRate = 100;
        double[]? psd = FftSharp.Transform.FFTpower(signal);
        double[]? freq = FftSharp.Transform.FFTfreq(sampleRate, psd.Length);
        int x = 0;
        foreach (double value in psd)
        {
            double rvalue = value;
            if (value < -380) rvalue = -380;
            FftInfo info = new(rvalue, freq[x]);
            values.Add(info);
            x++;
        }
    }

    private async Task<double[]> GetLastSamples(int windowSize = 256)
    {
        try
        {
            if (_samples.Count == 0) return Array.Empty<double>();
            double[] samplesList = _samples
                .OrderByDescending(sample => sample.Time)
                .Take(FftWindowSize)
                .Select(sampleRecord => sampleRecord.Sample)
                .ToArray();

            return await Task.FromResult(samplesList);
        }
        catch (Exception ex)
        {
            return Array.Empty<double>();
        }
    }

    private Thread GetThread()
    {
        Thread thread = new(Fft_BackgroundTask)
        {
            Priority = ThreadPriority.BelowNormal,
        };
        thread.Start();
        return thread;
    }

    private void UseMutex(Action action)
    {
        Mutex.WaitOne();
        action.Invoke();
        Mutex.ReleaseMutex();
    }
}

public class FftSample
{
    public double Sample { get; set; }

    public DateTime Time { get; set; }

    public FftSample(double sample, DateTime time)
    {
        Sample = sample;
        Time = time;
    }
}
