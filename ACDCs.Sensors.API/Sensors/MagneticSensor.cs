namespace ACDCs.Sensors.API.Sensors;

using System.Numerics;
using Interfaces;
using Microsoft.Maui.Devices.Sensors;

public class MagneticSensor : ISensor<Vector3>
{
    public static bool IsSupported
    {
        get { return Magnetometer.IsSupported; }
        set => throw new NotImplementedException();
    }

    public Action<Vector3>? OnReadingChanged { get; set; }

    public void Start()
    {
        Magnetometer.ReadingChanged += OnReadingChangedBase;
        Magnetometer.Start(SensorSpeed.Fastest);
    }

    public void Stop()
    {
        Magnetometer.Stop();
        Magnetometer.ReadingChanged -= OnReadingChangedBase;
    }

    private void OnReadingChangedBase(object? sender, MagnetometerChangedEventArgs e)
    {
        OnReadingChanged?.Invoke(e.Reading.MagneticField);
    }
}
