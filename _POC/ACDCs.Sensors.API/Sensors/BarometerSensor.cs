namespace ACDCs.Sensors.API.Sensors;

using Interfaces;

public class BarometerSensor : ISensor<double>
{
    public static bool Supported => Barometer.IsSupported;

    public bool IsSupported
    {
        get { return Barometer.IsSupported; }
        set => throw new NotImplementedException();
    }

    public Action<double>? OnReadingChanged { get; set; }

    public void Start()
    {
        Barometer.ReadingChanged += OnReadingChangedBase;
        Barometer.Start(SensorSpeed.Fastest);
    }

    public void Stop()
    {
        Barometer.Stop();
        Barometer.ReadingChanged -= OnReadingChangedBase;
    }

    private void OnReadingChangedBase(object? sender, BarometerChangedEventArgs e)
    {
        OnReadingChanged?.Invoke(e.Reading.PressureInHectopascals);
    }
}
