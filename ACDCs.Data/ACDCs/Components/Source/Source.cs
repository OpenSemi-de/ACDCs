using ACDCs.Data.ACDCs.Interfaces;

namespace ACDCs.Data.ACDCs.Components.Source;

public class Source : SourceParameters, IElectronicComponent
{
    public string Model { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public IComponentRuntimeParameters? ParametersRuntime => null;
    public string Type { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}
