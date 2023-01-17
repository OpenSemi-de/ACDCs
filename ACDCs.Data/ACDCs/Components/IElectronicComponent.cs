namespace ACDCs.Data.ACDCs.Components;

public interface IElectronicComponent
{
    string Model { get; set; }
    string Name { get; set; }
    public IComponentRuntimeParameters ParametersRuntime { get; }
    string Type { get; set; }

    string Value { get; set; }
}
