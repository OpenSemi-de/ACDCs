namespace ACDCs.Sensors.API.Interfaces;

public interface ISensorWorker<TSampleType>
{
    public bool Started { get; set; }
    public bool Supported { get; }

    List<TSampleType> GetSamples(DateTime? fromDate = null, int count = 256);

    void Start();

    void Stop();
}
