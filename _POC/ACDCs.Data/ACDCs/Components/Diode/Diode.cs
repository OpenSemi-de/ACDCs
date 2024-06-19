using ACDCs.Data.ACDCs.Interfaces;

namespace ACDCs.Data.ACDCs.Components.Diode;

public class Diode : DiodeParameters, IElectronicComponent
{
    public string Model { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public IComponentRuntimeParameters ParametersRuntime => new DiodeRuntimeParameters();
    public string Type { get; set; } = string.Empty;

    public string Value
    {
        get => Name; set => Name = value;
    }
}
