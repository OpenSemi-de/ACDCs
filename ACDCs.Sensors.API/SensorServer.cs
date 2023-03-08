// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace ACDCs.Sensors.API;

using Interfaces;
using Newtonsoft.Json;
using Samples;
using Sensors;
using WatsonWebserver;
using Worker;

// ReSharper disable once UnusedType.Global
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

    // ReSharper disable once NotAccessedField.Local
    private readonly Timer _updateTimer;

    private int _connectionCount;
    private int _sampleCount;
    private bool _started;
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
    public string? NetworkConnectionLabel { get; set; }
    public string? NetworkDeliveryLabel { get; set; }
    public string? OrientationSensorCacheLabelText { get; set; }
    public string? OrientationSensorSpeedLabelText { get; private set; }
    public bool OrientationSupported { get; set; }
    public bool OrientationWorkerStarted { get; private set; }

    public SensorServer(int port)
    {
        _server = new Server("*", port, defaultRoute: GetDefaultRoute);
        MagnetometerSupported = AddWorker<MagneticWorker, MagneticSample>(out _magneticWorker, GetMagneticSamples);
        OrientationSupported = AddWorker<OrientationWorker, OrientationSample>(out _orientationWorker, GetOrientationSamples);
        AccelerationSupported =
            AddWorker<AccelerationWorker, AccelerationSample>(out _accelerationWorker, GetAccelerationSamples);
        BarometerSupported = AddWorker<BarometerWorker, BarometerSample>(out _barometerWorker, GetBarometerSamples);
        CompassSupported = AddWorker<CompassWorker, CompassSample>(out _compassWorker, GetCompassSamples);
        GyroscopeSupported = AddWorker<GyroscopeWorker, GyroscopeSample>(out _gyroscopeWorker, GetGyroscopeSamples);
        AddAvailabilityRequest();
        _updateTimer = new Timer(UpdateGui, null, 0, 1000);
    }

    public void Start()
    {
        if (_started) return;
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

        _started = true;
    }

    public void Stop()
    {
        if (!_started) return;
        _server.Stop();

        _magneticWorker?.Stop();
        _accelerationWorker?.Stop();
        _orientationWorker?.Stop();
        _barometerWorker?.Stop();
        _compassWorker?.Stop();
        _gyroscopeWorker?.Stop();

        _threads.ForEach(t => t.Join());
        _threads.Clear();
        _started = false;
    }

    private void AddAvailabilityRequest()
    {
        _server.Routes.Static.Add(HttpMethod.GET, "/Sensors/Availability", GetSensorAvailability);
    }

    private bool AddWorker<T, TSampleType>(out T? worker, Func<HttpContext, Task> routeHandler) where T : ISensorWorker<TSampleType>, new()
    {
        worker = new T();
        _server.Routes.Static.Add(HttpMethod.GET, $"/{typeof(T).Name.Replace("Worker", "")}/Samples", routeHandler);
        return worker.Supported;
    }

    private async Task GetAccelerationSamples(HttpContext ctx)
    {
        _connectionCount++;
        if (_accelerationWorker != null)
        {
            List<AccelerationSample> accelerationSamples = _accelerationWorker.GetSamples();
            _sampleCount += accelerationSamples.Count;
            await ctx.Response.Send(JsonConvert.SerializeObject(accelerationSamples));
        }
    }

    private async Task GetBarometerSamples(HttpContext ctx)
    {
        _connectionCount++;
        if (_barometerWorker != null)
        {
            List<BarometerSample> barometerSamples = _barometerWorker.GetSamples();
            _sampleCount += barometerSamples.Count;
            await ctx.Response.Send(JsonConvert.SerializeObject(barometerSamples));
        }
    }

    private async Task GetCompassSamples(HttpContext ctx)
    {
        _connectionCount++;
        if (_compassWorker != null)
        {
            List<CompassSample> compassSamples = _compassWorker.GetSamples();
            _sampleCount += compassSamples.Count;
            await ctx.Response.Send(JsonConvert.SerializeObject(compassSamples));
        }
    }

    private int GetConnectionCount()
    {
        int count = _connectionCount;
        _connectionCount = 0;
        return count;
    }

    private async Task GetDefaultRoute(HttpContext ctx)
    {
        _connectionCount++;
        await ctx.Response.Send("ACDCs.Sensors.Server started.");
    }

    private async Task GetGyroscopeSamples(HttpContext ctx)
    {
        _connectionCount++;
        if (_gyroscopeWorker != null)
        {
            List<GyroscopeSample> gyroscopeSamples = _gyroscopeWorker.GetSamples();
            _sampleCount += gyroscopeSamples.Count;
            await ctx.Response.Send(JsonConvert.SerializeObject(gyroscopeSamples));
        }
    }

    private async Task GetMagneticSamples(HttpContext ctx)
    {
        _connectionCount++;
        if (_magneticWorker != null)
        {
            List<MagneticSample> magneticSamples = _magneticWorker.GetSamples();
            _sampleCount += magneticSamples.Count;
            await ctx.Response.Send(JsonConvert.SerializeObject(magneticSamples));
        }
    }

    private async Task GetOrientationSamples(HttpContext ctx)
    {
        _connectionCount++;
        if (_orientationWorker != null)
        {
            List<OrientationSample> orientationSamples = _orientationWorker.GetSamples();
            _sampleCount += orientationSamples.Count;
            await ctx.Response.Send(JsonConvert.SerializeObject(orientationSamples));
        }
    }

    private int GetSampleCount()
    {
        int count = _sampleCount;
        _sampleCount = 0;
        return count;
    }

    private async Task GetSensorAvailability(HttpContext ctx)
    {
        await ctx.Response.Send(JsonConvert.SerializeObject(SensorAvailability.GetAvailableSensors()));
    }

    private void StartThread(Action start)
    {
        Thread thread = new(start.Invoke);
        _threads.Add(thread);
        thread.Start();
    }

    private async void UpdateGui(object? state)
    {
        NetworkConnectionLabel = $"{GetConnectionCount()} connections/sec.";
        NetworkDeliveryLabel = $"{GetSampleCount()} samples/sec.";
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
