using ACDCs.Data.ACDCs.Interfaces;

namespace ACDCs.Data.ACDCs.Components.Source;

public class SourceParameters : IComponentParameters
{
    public double AcMagnitude { get; set; }
    public double AcPhase { get; set; }
    public double AcValue { get; set; }
    public double DcValue { get; set; }
    public Phasor Phasor { get; set; } = new();
    public object? Waveform { get; set; }
}
