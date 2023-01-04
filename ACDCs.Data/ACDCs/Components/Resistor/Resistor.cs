namespace ACDCs.Data.ACDCs.Components.Resistor;

public class Resistor : ResistorParameters, IElectronicComponent
{
    public string Model { get; set; }
    public string Name { get; set; }
    public IComponentParameters ParametersModel => new ResistorParameters();
    public IComponentRuntimeParameters ParametersRuntime => throw new NotImplementedException();
    public string Type { get; set; }
}
