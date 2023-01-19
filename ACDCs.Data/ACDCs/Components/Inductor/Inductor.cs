using ACDCs.Data.ACDCs.Interfaces;

namespace ACDCs.Data.ACDCs.Components.Inductor;

public class Inductor : InductorParameters, IElectronicComponent
{
    private string _model = string.Empty;

    public string Model
    {
        get => _model;
        set => _model = value;
    }

    public string Name { get; set; } = string.Empty;

    public IComponentRuntimeParameters ParametersRuntime { get; } = new InductorRuntimeParameters();
    public string Type { get; set; } = string.Empty;

    public string Value { get; set; } = string.Empty;
}
