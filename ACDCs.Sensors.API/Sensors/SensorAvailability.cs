namespace ACDCs.Sensors.API.Sensors;

public static class SensorAvailability
{
    public static Dictionary<Type, bool> GetAvailableSensors()
    {
        return new Dictionary<Type, bool>
        {
            { typeof(AccelerationSensor), AccelerationSensor.Supported },
            { typeof(BarometerSensor), BarometerSensor.Supported },
            { typeof(CompassSensor), CompassSensor.Supported },
            { typeof(GyroscopeSensor), GyroscopeSensor.Supported },
            { typeof(MagneticSensor), MagneticSensor.Supported },
            { typeof(OrientationSensor), OrientationSensor.Supported },
        };
    }
}
