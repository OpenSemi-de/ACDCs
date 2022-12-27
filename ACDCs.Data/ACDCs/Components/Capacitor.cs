namespace ACDCs.Data.ACDCs.Components;

public class Capacitor : CapacitorParameters, IElectronicComponent
{
    public string Model { get; set; }
    public new string Name { get; set; }
    public IComponentRuntimeParameters ParametersRuntime => new CapacitorRuntimeParameters();
    public string Type { get; set; }
}

public class CapacitorParameters : IComponentParameters
{
    public float DefaultWidth { get; set; }
    public float JunctionCap { get; set; }
    public float JunctionCapSidewall { get; set; }
    public float Narrow { get; set; }
    public float NominalTemperature { get; set; }
    public float NominalTemperatureCelsius { get; set; }
    public float TemperatureCoefficient1 { get; set; }
    public float TemperatureCoefficient2 { get; set; }
}

public class CapacitorRuntimeParameters : IComponentRuntimeParameters
{
    public float Capacitance { get; set; }
    public float InitialCondition { get; set; }
    public float Length { get; set; }
    public float ParallelMultiplier { get; set; }
    public float Temperature { get; set; }
    public float TemperatureCelsius { get; set; }
    public float Width { get; set; }
}
