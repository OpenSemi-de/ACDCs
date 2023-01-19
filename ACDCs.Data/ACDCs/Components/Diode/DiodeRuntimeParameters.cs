using ACDCs.Data.ACDCs.Interfaces;

namespace ACDCs.Data.ACDCs.Components.Diode;

public class DiodeRuntimeParameters : IComponentRuntimeParameters
{
    public double Area { get; set; }
    public double InitCond { get; set; }
    public bool Off { get; set; }
    public double ParallelMultiplier { get; set; }
    public double SeriesMultiplier { get; set; }
    public double Temperature { get; set; }
    public double TemperatureCelsius { get; set; }
}
