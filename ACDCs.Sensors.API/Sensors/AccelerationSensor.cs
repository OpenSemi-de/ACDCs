namespace ACDCs.Sensors.API.Sensors;

using System.Numerics;
using Interfaces;
using Microsoft.Maui.Devices.Sensors;

public class AccelerationSensor : ISensor<Vector3>
{
    public bool IsSupported
    {
        get { return Accelerometer.IsSupported; }
        set => throw new NotImplementedException();
    }

    public Action<Vector3>? OnReadingChanged { get; set; }

    public void Start()
    {
        Accelerometer.ReadingChanged += OnReadingChangedBase;
        Accelerometer.Start(SensorSpeed.Fastest);
    }

    public void Stop()
    {
        Accelerometer.Stop();
        Accelerometer.ReadingChanged -= OnReadingChangedBase;
    }

    private void OnReadingChangedBase(object? sender, AccelerometerChangedEventArgs e)
    {
        OnReadingChanged?.Invoke(e.Reading.Acceleration);
    }
}
