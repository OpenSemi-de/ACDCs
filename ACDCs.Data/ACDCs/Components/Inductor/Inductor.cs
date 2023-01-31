using ACDCs.Data.ACDCs.Interfaces;

namespace ACDCs.Data.ACDCs.Components.Inductor;

public class Inductor : InductorParameters, IElectronicComponent
{
    public string Model { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public IComponentRuntimeParameters ParametersRuntime { get; } = new InductorRuntimeParameters();
    public string Type { get; set; } = string.Empty;

    public string Value
    {
        get => Convert.ToString(((InductorRuntimeParameters)ParametersRuntime).Inductance);
        set
        {
            if (ParametersRuntime is InductorRuntimeParameters inductorRuntimeParameters)
            {
                inductorRuntimeParameters.Inductance = Convert.ToDouble(value);
            }
        }
    }
}
