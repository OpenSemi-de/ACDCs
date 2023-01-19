using ACDCs.Data.ACDCs.Interfaces;

namespace ACDCs.Data.ACDCs.Components.JFET;

public class JFETParameters : IComponentParameters
{
    public double B { get; set; }
    public double Beta { get; set; }
    public double CapGd { get; set; }
    public double CapGs { get; set; }
    public double DepletionCapCoefficient { get; set; }
    public double DrainConductance { get; set; }
    public double DrainResistance { get; set; }
    public double FnCoefficient { get; set; }
    public double FnExponent { get; set; }
    public double GatePotential { get; set; }
    public double GateSaturationCurrent { get; set; }
    public double JFETType { get; set; }
    public double LModulation { get; set; }
    public double NominalTemperature { get; set; }
    public double NominalTemperatureCelsius { get; set; }
    public double SourceConductance { get; set; }
    public double SourceResistance { get; set; }
    public double Threshold { get; set; }
    public string TypeName { get; set; } = string.Empty;
}
