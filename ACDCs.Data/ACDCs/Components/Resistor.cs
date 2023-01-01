namespace ACDCs.Data.ACDCs.Components;

public class Resistor : ResistorParameters, IElectronicComponent
{
    public string Model { get; set; }
    public string Name { get; set; }
    public IComponentParameters ParametersModel => new ResistorParameters();
    public IComponentRuntimeParameters ParametersRuntime => throw new NotImplementedException();
    public string Type { get; set; }
}

public class ResistorParameters : IComponentParameters
{
    public double DefaultWidth { get; set; }
    public double ExponentialCoefficient { get; set; }
    public double Narrow { get; set; }
    public double NominalTemperature { get; set; }
    public double NominalTemperatureCelsius { get; set; }
    public double SheetResistance { get; set; }
    public double TemperatureCoefficient1 { get; set; }
    public double TemperatureCoefficient2 { get; set; }
}
