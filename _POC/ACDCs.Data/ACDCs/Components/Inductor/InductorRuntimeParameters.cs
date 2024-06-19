using System.Diagnostics.CodeAnalysis;
using ACDCs.Data.ACDCs.Interfaces;

namespace ACDCs.Data.ACDCs.Components.Inductor;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class InductorRuntimeParameters : IComponentRuntimeParameters
{
    public double Inductance { get; set; }
    public double InitialCondition { get; set; }
    public double ParallelMultiplier { get; set; }
    public double SeriesMultiplier { get; set; }
}
