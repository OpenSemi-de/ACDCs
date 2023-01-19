using ACDCs.Data.ACDCs.Interfaces;

namespace ACDCs.Data.ACDCs.Components.Capacitor;

public class CapacitorRuntimeParameters : IComponentRuntimeParameters
{
    public double Capacitance { get; set; }
    public double InitialCondition { get; set; }
    public double Length { get; set; }
    public double ParallelMultiplier { get; set; }
    public double Temperature { get; set; }
    public double TemperatureCelsius { get; set; }
    public double Width { get; set; }
}
