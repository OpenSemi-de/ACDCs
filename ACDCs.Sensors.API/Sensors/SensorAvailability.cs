namespace ACDCs.Sensors.API.Sensors;

public static class SensorAvailability
{
    public static Dictionary<Type, bool> GetAvailableSensors()
    {
        return new Dictionary<Type, bool>
        {
            { typeof(AccelerationSensor), AccelerationSensor.IsSupported },
            { typeof(BarometerSensor), BarometerSensor.IsSupported },
            { typeof(CompassSensor), CompassSensor.IsSupported },
            { typeof(GyroscopeSensor), GyroscopeSensor.IsSupported },
            { typeof(MagneticSensor), MagneticSensor.IsSupported },
            { typeof(OrientationSensor), OrientationSensor.IsSupported },
        };
    }
}
