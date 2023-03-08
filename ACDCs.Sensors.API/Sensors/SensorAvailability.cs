namespace ACDCs.Sensors.API.Sensors;

public static class SensorAvailability
{
    public static List<SensorItem> GetAvailableSensors()
    {
        Dictionary<Type, bool> supported = new()
        {
            { typeof(AccelerationSensor), AccelerationSensor.Supported },
            { typeof(BarometerSensor), BarometerSensor.Supported },
            { typeof(CompassSensor), CompassSensor.Supported },
            { typeof(GyroscopeSensor), GyroscopeSensor.Supported },
            { typeof(MagneticSensor), MagneticSensor.Supported },
            { typeof(OrientationSensor), OrientationSensor.Supported },
        };

        List<SensorItem> available = supported
            .Where(kv => kv.Value)
            .Select(kv => new SensorItem(
                kv.Key.Name,
                $"/{kv.Key.Name.Replace("Sensor", "")}/Samples",
                kv.Key.Name,
                SensorSpeed.Fastest,
                kv.Key.Name,
                kv.Key.Name))
            .ToList();
        return available;
    }
}
