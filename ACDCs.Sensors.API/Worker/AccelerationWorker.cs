namespace ACDCs.Sensors.API.Worker;

using System.Numerics;
using Samples;
using Sensors;

public class AccelerationWorker : SensorWorker<Vector3, AccelerationSample>
{
    public AccelerationWorker() : base(new AccelerationSensor())
    {
    }
}
