using ACDCs.Data.ACDCs.Interfaces;

namespace ACDCs.Data.ACDCs.Components.JFET;

public class JFETRuntimeParameters : IComponentRuntimeParameters
{
    public double Area { get; set; }
    public double InitialVds { get; set; }
    public double InitialVgs { get; set; }
    public bool Off { get; set; }
    public double ParallelMultiplier { get; set; }
    public double Temperature { get; set; }
    public double TemperatureCelsius { get; set; }
}
