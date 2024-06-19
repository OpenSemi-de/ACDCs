using ACDCs.Data.ACDCs.Interfaces;

namespace ACDCs.Data.ACDCs.Components.Capacitor;

public class Capacitor : CapacitorParameters, IElectronicComponent
{
    public string Model { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public IComponentRuntimeParameters ParametersRuntime => new CapacitorRuntimeParameters();
    public string Type { get; set; } = string.Empty;

    public string Value
    {
        get => Convert.ToString(((CapacitorRuntimeParameters)ParametersRuntime).Capacitance);
        set
        {
            if (ParametersRuntime is CapacitorRuntimeParameters capacitorRuntimeParameters)
            {
                capacitorRuntimeParameters.Capacitance = Convert.ToDouble(value);
            }
        }
    }
}
