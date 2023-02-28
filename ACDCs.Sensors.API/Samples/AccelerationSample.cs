namespace ACDCs.Sensors.API.Samples;

using System.Numerics;
using Interfaces;

public class AccelerationSample : ISample<Vector3>
{
    public Vector3 Sample { get; set; }

    public DateTime Time { get; set; }
}
