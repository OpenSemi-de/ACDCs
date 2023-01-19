using ACDCs.Data.ACDCs.Interfaces;

namespace ACDCs.Data.ACDCs.Components.Inductor;

public class InductorParameters : IComponentParameters
{
    public float Inductance { get; set; }
    public float InitialCondition { get; set; }
    public float ParallelMultiplier { get; set; }
    public float SeriesMultiplier { get; set; }
}
