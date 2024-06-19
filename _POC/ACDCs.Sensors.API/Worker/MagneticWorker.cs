namespace ACDCs.Sensors.API.Worker;

using System.Numerics;
using Interfaces;
using Samples;
using Sensors;

public class MagneticWorker : SensorWorker<Vector3, MagneticSample>, ISensorWorker<MagneticSample>
{
    public MagneticWorker() : base(new MagneticSensor())
    {
    }
}
