namespace ACDCs.Sensors.API.Worker;

using System.Numerics;
using Samples;
using Sensors;

public class GyroscopeWorker : SensorWorker<Vector3, GyroscopeSample>
{
    public GyroscopeWorker() : base(new GyroscopeSensor())
    {
    }
}
