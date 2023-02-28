namespace ACDCs.Sensors.API;

using Interfaces;
using Newtonsoft.Json;
using Samples;
using WatsonWebserver;
using Worker;

public class SensorServer
{
    private readonly AccelerationWorker? _accelerationWorker;
    private readonly BarometerWorker? _barometerWorker;
    private readonly CompassWorker? _compassWorker;
    private readonly GyroscopeWorker? _gyroscopeWorker;
    private readonly MagneticWorker? _magneticWorker;
    private readonly OrientationWorker? _orientationWorker;
    private readonly Server _server;
    private readonly List<Thread> _threads = new();
    private readonly Timer _updateTimer;
    public bool AccelerationSupported { get; set; }
    public bool AccelerationWorkerStarted { get; private set; }
    public string? AccelerometerCacheLabelText { get; set; }
    public string? AccelerometerSpeedLabelText { get; private set; }
    public string? BarometerSensorCacheLabelText { get; set; }
    public string? BarometerSensorSpeedLabelText { get; private set; }
    public bool BarometerSupported { get; set; }
    public bool BarometerWorkerStarted { get; private set; }
    public string? CompassSensorCacheLabelText { get; set; }
    public string? CompassSensorSpeedLabelText { get; private set; }
    public bool CompassSupported { get; set; }
    public bool CompassWorkerStarted { get; private set; }
    public string? GyroscopeSensorCacheLabelText { get; set; }
    public string? GyroscopeSensorSpeedLabelText { get; private set; }
    public bool GyroscopeSupported { get; set; }
    public bool GyroscopeWorkerStarted { get; private set; }
    public bool MagneticWorkerStarted { get; private set; }
    public string? MagnetometerCacheLabelText { get; set; }
    public string? MagnetometerSpeedLabelText { get; private set; }
    public bool MagnetometerSupported { get; set; }
    public string? OrientationSensorCacheLabelText { get; set; }
    public string? OrientationSensorSpeedLabelText { get; private set; }
    public bool OrientationSupported { get; set; }
    public bool OrientationWorkerStarted { get; private set; }

    private bool Started { get; set; }

    public SensorServer()
    {
        _server = new Server("*", 5000, defaultRoute: GetDefaultRoute);

        MagnetometerSupported = AddWorker<MagneticWorker, MagneticSample>(out _magneticWorker, GetMagneticSamples);
        OrientationSupported = AddWorker<OrientationWorker, OrientationSample>(out _orientationWorker, GetOrientationSamples);
        AccelerationSupported =
            AddWorker<AccelerationWorker, AccelerationSample>(out _accelerationWorker, GetAccelerationSamples);
        BarometerSupported = AddWorker<BarometerWorker, BarometerSample>(out _barometerWorker, GetBarometerSamples);
        CompassSupported = AddWorker<CompassWorker, CompassSample>(out _compassWorker, GetCompassSamples);
        GyroscopeSupported = AddWorker<GyroscopeWorker, GyroscopeSample>(out _gyroscopeWorker, GetGyroscopeSamples);

        _updateTimer = new Timer(UpdateGui, null, 0, 1000);
    }

    public void Start()
    {
        if (Started) return;
        _server.Start();

        if (MagnetometerSupported)
            if (_magneticWorker != null)
            {
                StartThread(_magneticWorker.Start);
            }

        if (AccelerationSupported)
            if (_accelerationWorker != null)
            {
                StartThread(_accelerationWorker.Start);
            }

        if (BarometerSupported)
            if (_barometerWorker != null)
            {
                StartThread(_barometerWorker.Start);
            }

        if (OrientationSupported)
            if (_orientationWorker != null)
            {
                StartThread(_orientationWorker.Start);
            }

        if (CompassSupported)
            if (_compassWorker != null)
            {
                StartThread(_compassWorker.Start);
            }

        if (GyroscopeSupported)
            if (_gyroscopeWorker != null)
            {
                StartThread(_gyroscopeWorker.Start);
            }

        Thread.Sleep(500);
        if (_magneticWorker != null)
        {
            MagneticWorkerStarted = _magneticWorker.Started;
        }

        if (_orientationWorker != null)
        {
            OrientationWorkerStarted = _orientationWorker.Started;
        }

        if (_accelerationWorker != null)
        {
            AccelerationWorkerStarted = _accelerationWorker.Started;
        }

        if (_barometerWorker != null)
        {
            BarometerWorkerStarted = _barometerWorker.Started;
        }

        if (_compassWorker != null)
        {
            CompassWorkerStarted = _compassWorker.Started;
        }

        if (_gyroscopeWorker != null)
        {
            GyroscopeWorkerStarted = _gyroscopeWorker.Started;
        }

        Started = true;
    }

    public void Stop()
    {
        if (!Started) return;
        _server.Stop();

        _magneticWorker.Stop();
        _accelerationWorker.Stop();
        _orientationWorker.Stop();
        _barometerWorker.Stop();
        _compassWorker.Stop();
        _gyroscopeWorker.Stop();

        _threads.ForEach(t => t.Join());
        _threads.Clear();
        Started = false;
    }

    private bool AddWorker<T, TSampleType>(out T? worker, Func<HttpContext, Task> routeHandler) where T : ISensorWorker<TSampleType>, new()
    {
        worker = new T();
        _server.Routes.Static.Add(HttpMethod.GET, $"/{typeof(T).Name.Replace("Worker", "")}/Samples", routeHandler);
        return worker.Supported;
    }

    private async Task GetAccelerationSamples(HttpContext ctx)
    {
        await ctx.Response.Send(JsonConvert.SerializeObject(_accelerationWorker.GetSamples()));
    }

    private async Task GetBarometerSamples(HttpContext ctx)
    {
        await ctx.Response.Send(JsonConvert.SerializeObject(_barometerWorker.GetSamples()));
    }

    private async Task GetCompassSamples(HttpContext ctx)
    {
        await ctx.Response.Send(JsonConvert.SerializeObject(_compassWorker.GetSamples()));
    }

    private async Task GetDefaultRoute(HttpContext ctx)
    {
        await ctx.Response.Send("ACDCs.Sensors.Server started.");
    }

    private async Task GetGyroscopeSamples(HttpContext ctx)
    {
        await ctx.Response.Send(JsonConvert.SerializeObject(_gyroscopeWorker.GetSamples()));
    }

    private async Task GetMagneticSamples(HttpContext ctx)
    {
        await ctx.Response.Send(JsonConvert.SerializeObject(_magneticWorker.GetSamples()));
    }

    private async Task GetOrientationSamples(HttpContext ctx)
    {
        await ctx.Response.Send(JsonConvert.SerializeObject(_orientationWorker.GetSamples()));
    }

    private void StartThread(Action start)
    {
        Thread thread = new(start.Invoke);
        _threads.Add(thread);
        thread.Start();
    }

    private async void UpdateGui(object? state)
    {
        AccelerometerSpeedLabelText = $"{_accelerationWorker.NumberOfSamplesSinceLastCheck()} samples/sec.";
        MagnetometerSpeedLabelText = $"{_magneticWorker.NumberOfSamplesSinceLastCheck()} samples/sec.";
        OrientationSensorSpeedLabelText = $"{_orientationWorker.NumberOfSamplesSinceLastCheck()} samples/sec.";
        BarometerSensorSpeedLabelText = $"{_barometerWorker.NumberOfSamplesSinceLastCheck()} samples/sec.";
        CompassSensorSpeedLabelText = $"{_compassWorker.NumberOfSamplesSinceLastCheck()} samples/sec.";
        GyroscopeSensorSpeedLabelText = $"{_gyroscopeWorker.NumberOfSamplesSinceLastCheck()} samples/sec.";
        AccelerometerCacheLabelText = $"{_accelerationWorker.NumberOfSamplesInCache()} samples/cache";
        MagnetometerCacheLabelText = $"{_magneticWorker.NumberOfSamplesInCache()} samples/cache";
        OrientationSensorCacheLabelText = $"{_orientationWorker.NumberOfSamplesInCache()} samples/cache";
        BarometerSensorCacheLabelText = $"{_barometerWorker.NumberOfSamplesInCache()} samples/cache";
        CompassSensorCacheLabelText = $"{_compassWorker.NumberOfSamplesInCache()} samples/cache";
        GyroscopeSensorCacheLabelText = $"{_gyroscopeWorker.NumberOfSamplesInCache()} samples/cache";
        await Task.CompletedTask;
    }
}
