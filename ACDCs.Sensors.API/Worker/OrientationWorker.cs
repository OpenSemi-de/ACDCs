namespace ACDCs.Sensors.API.Worker;

using System.Numerics;
using Samples;
using Sensors;

public class OrientationWorker : SensorWorker<Quaternion, OrientationSample>
{
    public OrientationWorker() : base(new OrientationSensor())
    {
    }
}
