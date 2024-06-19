namespace ACDCs.Sensors.API.Worker;

using Samples;
using Sensors;

public class BarometerWorker : SensorWorker<double, BarometerSample>
{
    public BarometerWorker() : base(new BarometerSensor())
    {
    }
}
