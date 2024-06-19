namespace ACDCs.Sensors.API.Samples;

using System.Numerics;
using Interfaces;

public class OrientationSample : ISample<Quaternion>, ISample
{
    public Quaternion Sample { get; set; }

    public DateTime Time { get; set; }
}
