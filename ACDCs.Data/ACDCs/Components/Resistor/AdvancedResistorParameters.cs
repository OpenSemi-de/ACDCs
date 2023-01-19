using System.Diagnostics.CodeAnalysis;

namespace ACDCs.Data.ACDCs.Components.Resistor;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class AdvancedResistorParameters
{
    public double Multiplicator { get; set; }
    public string ResistorType { get; set; } = string.Empty;
    public ResistorSeries Series { get; set; }
    public double Tolerance { get; set; }
}
