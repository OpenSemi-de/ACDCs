namespace ACDCs.Sensors.API.Interfaces;

public interface ISensor<TResult>
{
    public bool IsSupported { get; set; }
    public Action<TResult>? OnReadingChanged { get; set; }

    public void Start();

    public void Stop();
}
