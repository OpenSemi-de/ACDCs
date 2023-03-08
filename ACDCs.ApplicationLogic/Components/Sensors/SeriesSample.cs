namespace ACDCs.API.Core.Components.Sensors;

public class SeriesSample
{
    public DateTime Time { get; set; }

    public double Value { get; set; }

    public SeriesSample()
    {
    }

    public SeriesSample(double value, DateTime time)
    {
        Value = value;
        Time = time;
    }
}
