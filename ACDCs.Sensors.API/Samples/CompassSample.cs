namespace ACDCs.Sensors.API.Samples;

using Interfaces;

public class CompassSample : ISample<double>, ISample
{
    public double Sample { get; set; }
    public DateTime Time { get; set; }
}
