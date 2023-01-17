namespace ACDCs.Data.ACDCs.Components;

public class JFET : JFETParameters, IElectronicComponent
{
    public string Model { get; set; }
    public string Name { get; set; }
    public IComponentRuntimeParameters ParametersRuntime => new JFETParametersRuntimeParameters();
    public string Type { get; set; }

    public string Value
    {
        get => Name;
        set => Name = value;
    }
}

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
    public string TypeName { get; set; }
}

public class JFETParametersRuntimeParameters : IComponentRuntimeParameters
{
    public double Area { get; set; }
    public double InitialVds { get; set; }
    public double InitialVgs { get; set; }
    public bool Off { get; set; }
    public double ParallelMultiplier { get; set; }
    public double Temperature { get; set; }
    public double TemperatureCelsius { get; set; }
}
