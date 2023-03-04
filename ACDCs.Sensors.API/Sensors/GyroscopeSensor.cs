namespace ACDCs.Sensors.API.Sensors;

using System.Numerics;
using Interfaces;

public class GyroscopeSensor : ISensor<Vector3>
{
    public static bool IsSupported
    {
        get { return Gyroscope.IsSupported; }
        set => throw new NotImplementedException();
    }

    public Action<Vector3>? OnReadingChanged { get; set; }

    public void Start()
    {
        Gyroscope.ReadingChanged += OnReadingChangedBase;
        Gyroscope.Start(SensorSpeed.Fastest);
    }

    public void Stop()
    {
        Gyroscope.Stop();
        Gyroscope.ReadingChanged -= OnReadingChangedBase;
    }

    private void OnReadingChangedBase(object? sender, GyroscopeChangedEventArgs e)
    {
        OnReadingChanged?.Invoke(e.Reading.AngularVelocity);
    }
}
