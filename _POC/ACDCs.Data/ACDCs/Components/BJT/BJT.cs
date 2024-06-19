using ACDCs.Data.ACDCs.Interfaces;

namespace ACDCs.Data.ACDCs.Components.BJT;

public class Bjt : BjtModelParameters, IElectronicComponent
{
    public string Model { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public IComponentRuntimeParameters ParametersRuntime => new BjtRuntimeParameters();
    public string Type { get; set; } = string.Empty;

    public string Value
    {
        get => Name;
        set => Name = value;
    }
}
