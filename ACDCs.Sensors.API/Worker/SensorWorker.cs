namespace ACDCs.Sensors.API.Worker;

using System.Collections.Concurrent;
using Interfaces;

public class SensorWorker<TResult, TSampleType> : ISensorWorker<TSampleType> where TSampleType : ISample<TResult>, new()
{
    private readonly SensorWorker<TResult, TSampleType> _instance;
    private readonly ConcurrentDictionary<DateTime, TSampleType> _samples = new();
    private readonly ISensor<TResult> _sensor;
    private Thread? _cleanupThread;
    private int _numberOfSamples;
    public bool Started { get; set; }
    public bool Supported => _sensor.IsSupported;

    protected SensorWorker(ISensor<TResult> sensor)
    {
        _sensor = sensor;
        _instance = this;
    }

    private delegate bool TryRemoveThreaded(DateTime key, out TSampleType value);

    public List<TSampleType> GetSamples(int count = 1024)
    {
        return _samples.AsParallel().OrderByDescending(s => s.Key)
            .Take(count)
            .Select(kv => kv.Value)
            .ToList();
    }

    public int NumberOfSamplesInCache()
    {
        return _samples.Count;
    }

    public int NumberOfSamplesSinceLastCheck()
    {
        int sampleCount = _numberOfSamples;
        _numberOfSamples = 0;
        return sampleCount;
    }

    public void Start()
    {
        if (Started) return;
        _sensor.OnReadingChanged = OnReadingChanged;
        _sensor.Start();
        Started = true;
        _cleanupThread = new Thread(CleanupJob);
        _cleanupThread.Start();
    }

    public void Stop()
    {
        if (!Started) return;
        _sensor.Stop();
        _sensor.OnReadingChanged = null;
        Started = false;
        _cleanupThread = null;
    }

    private void CleanupJob()
    {
        while (Started)
        {
            List<DateTime> list = _samples.Keys.Order().ToList();

            while (_samples.Count > 1000)
            {
                DateTime dateTime = list.First();
                var inv = new TryRemoveThreaded(_samples.TryRemove);
                inv.Invoke(dateTime, out _);
                list.Remove(dateTime);
            }

            Thread.Sleep(100);
        }
    }

    private async void OnReadingChanged(TResult result)
    {
        TSampleType sample = new() { Time = DateTime.UtcNow, Sample = result };
        _samples.GetOrAdd(sample.Time, sample);
        _numberOfSamples++;
        await Task.CompletedTask;
    }
}
