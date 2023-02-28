namespace ACDCs.Sensors.API.Sensors;

using System.Numerics;
using Interfaces;

public class OrientationSensor : ISensor<Quaternion>
{
    public bool IsSupported
    {
        get { return Microsoft.Maui.Devices.Sensors.OrientationSensor.IsSupported; }
        set => throw new NotImplementedException();
    }

    public Action<Quaternion>? OnReadingChanged { get; set; }

    public void Start()
    {
        Microsoft.Maui.Devices.Sensors.OrientationSensor.ReadingChanged += OnReadingChangedBase;
        Microsoft.Maui.Devices.Sensors.OrientationSensor.Start(SensorSpeed.Fastest);
    }

    public void Stop()
    {
        Microsoft.Maui.Devices.Sensors.OrientationSensor.Stop();
        Microsoft.Maui.Devices.Sensors.OrientationSensor.ReadingChanged -= OnReadingChangedBase;
    }

    private void OnReadingChangedBase(object? sender, OrientationSensorChangedEventArgs e)
    {
        OnReadingChanged?.Invoke(e.Reading.Orientation);
    }
}
