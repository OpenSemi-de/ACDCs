namespace ACDCs.Sensors.API.Client;

using Samples;

public class DownloadWorker
{
    private readonly List<DownloadClient> _clientsInstance = new();
    private readonly string? _outputPath;
    private readonly List<Thread> _threads = new();
    private readonly Uri? _uri;
    private Dictionary<Uri, IDownloadClient> _clients = new();
    private Dictionary<Uri, Type> _resultTypes = new();

    public DownloadWorker(string? outputPath, Uri? uri)
    {
        _outputPath = outputPath;
        _uri = uri;
    }

    public async Task Start()
    {
        if (_outputPath == null || _uri == null) return;

        Task.WaitAny(new Task[]
        {
            StartClient("/Magnetic/Samples", typeof(MagneticSample[])),
            StartClient("/Orientation/Samples", typeof(OrientationSample[])),
            StartClient("/Acceleration/Samples", typeof(AccelerationSample[])),
            StartClient("/Barometer/Samples", typeof(BarometerSample[])),
            Task.Delay(1000)
        });

        await Task.CompletedTask;
    }

    public void Stop()
    {
        foreach (var client in _clientsInstance)
        {
            client.Stop();
        }
    }

    private async Task StartClient(string url, Type resultType)
    {
        if (_outputPath == null || _uri == null) return;

        Uri clientUri = new(_uri, url);
        DownloadClient client = new(clientUri, _outputPath, resultType, 250);
        Thread newThread = new(() =>
        {
            client.Start().Wait();
        })
        { Priority = ThreadPriority.BelowNormal };
        newThread.Start();
        _threads.Add(newThread);
        _clientsInstance.Add(client);
        await Task.CompletedTask;
    }
}
