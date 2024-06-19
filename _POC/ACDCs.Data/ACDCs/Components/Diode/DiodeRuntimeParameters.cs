using System.Diagnostics.CodeAnalysis;
using ACDCs.Data.ACDCs.Interfaces;

namespace ACDCs.Data.ACDCs.Components.Diode;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
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
