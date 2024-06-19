using ACDCs.Data.ACDCs.Interfaces;

namespace ACDCs.Data.ACDCs.Components.Mosfet;

public class MosFetRuntimeParameters : IComponentRuntimeParameters
{
    public float DeltaTemperature { get; set; }
    public float DrainArea { get; set; }
    public float DrainPerimeter { get; set; }
    public float DrainSquares { get; set; }
    public float InitialVbs { get; set; }
    public float InitialVds { get; set; }
    public float InitialVgs { get; set; }
    public float Length { get; set; }
    public bool Off { get; set; }
    public float ParallelMultiplier { get; set; }
    public float SourceArea { get; set; }
    public float SourcePerimeter { get; set; }
    public float SourceSquares { get; set; }
    public float Temperature { get; set; }
    public float TemperatureCelsius { get; set; }
    public float Width { get; set; }
}
