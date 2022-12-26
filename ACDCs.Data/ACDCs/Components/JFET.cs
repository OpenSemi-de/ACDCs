namespace ACDCs.Data.ACDCs.Components;

public class JFET : ElectricalComponent, IElectronicComponent
{
    public IComponentParameters ParametersModel => new JFETParameters();
    public IComponentRuntimeParameters ParametersRuntime => new JFETParametersRuntimeParameters();
}

public class JFETParameters : IComponentParameters
{
    public float B { get; set; }
    public float Beta { get; set; }
    public float CapGd { get; set; }
    public float CapGs { get; set; }
    public float DepletionCapCoefficient { get; set; }
    public float DrainConductance { get; set; }
    public float DrainResistance { get; set; }
    public float FnCoefficient { get; set; }
    public float FnExponent { get; set; }
    public float GatePotential { get; set; }
    public float GateSaturationCurrent { get; set; }
    public float JFETType { get; set; }
    public float LModulation { get; set; }
    public float NominalTemperature { get; set; }
    public float NominalTemperatureCelsius { get; set; }
    public float SourceConductance { get; set; }
    public float SourceResistance { get; set; }
    public float Threshold { get; set; }
    public string TypeName { get; set; }
}

public class JFETParametersRuntimeParameters : IComponentRuntimeParameters
{
    public float Area { get; set; }
    public float InitialVds { get; set; }
    public float InitialVgs { get; set; }
    public bool Off { get; set; }
    public float ParallelMultiplier { get; set; }
    public float Temperature { get; set; }
    public float TemperatureCelsius { get; set; }
}
