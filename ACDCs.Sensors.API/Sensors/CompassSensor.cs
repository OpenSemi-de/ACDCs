namespace ACDCs.Sensors.API.Sensors;

using Interfaces;

public class CompassSensor : ISensor<double>
{
    public static bool IsSupported
    {
        get { return Compass.IsSupported; }
        set => throw new NotImplementedException();
    }

    public Action<double>? OnReadingChanged { get; set; }

    public void Start()
    {
        Compass.ReadingChanged += OnReadingChangedBase;
        Compass.Start(SensorSpeed.Fastest);
    }

    public void Stop()
    {
        Compass.Stop();
        Compass.ReadingChanged -= OnReadingChangedBase;
    }

    private void OnReadingChangedBase(object? sender, CompassChangedEventArgs e)
    {
        OnReadingChanged?.Invoke(e.Reading.HeadingMagneticNorth);
    }
}
