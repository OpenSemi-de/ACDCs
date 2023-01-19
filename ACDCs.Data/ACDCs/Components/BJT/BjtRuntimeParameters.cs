using System.Diagnostics.CodeAnalysis;
using ACDCs.Data.ACDCs.Interfaces;

namespace ACDCs.Data.ACDCs.Components.BJT;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class BjtRuntimeParameters : IComponentRuntimeParameters
{
    public double Area { get; set; }
    public double InitialVoltageBe { get; set; }
    public double InitialVoltageCe { get; set; }
    public bool Off { get; set; }
    public double ParallelMultiplier { get; set; }
    public double Temperature { get; set; }
    public double TemperatureCelsius { get; set; }
}
