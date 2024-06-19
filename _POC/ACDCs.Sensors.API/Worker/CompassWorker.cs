namespace ACDCs.Sensors.API.Worker;

using Samples;
using Sensors;

public class CompassWorker : SensorWorker<double, CompassSample>
{
    public CompassWorker() : base(new CompassSensor())
    {
    }
}
