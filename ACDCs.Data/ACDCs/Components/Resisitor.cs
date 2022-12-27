namespace ACDCs.Data.ACDCs.Components;

public class Resisitor : ResisitorParameters, IElectronicComponent
{
    public string Model { get; set; }
    public string Name { get; set; }
    public IComponentParameters ParametersModel => new ResisitorParameters();
    public IComponentRuntimeParameters ParametersRuntime => throw new NotImplementedException();
    public string Type { get; set; }
}

public class ResisitorParameters : IComponentParameters
{
    public float DefaultWidth { get; set; }
    public float ExponentialCoefficient { get; set; }
    public float Narrow { get; set; }
    public float NominalTemperature { get; set; }
    public float NominalTemperatureCelsius { get; set; }
    public float SheetResistance { get; set; }
    public float TemperatureCoefficient1 { get; set; }
    public float TemperatureCoefficient2 { get; set; }
}
