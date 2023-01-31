using ACDCs.Data.ACDCs.Interfaces;

namespace ACDCs.Data.ACDCs.Components.Mosfet;

public class MosFet1 : MosFetParametersLevel1, IElectronicComponent
{
    public string Model { get; set; }
    public string Name { get; set; }

    public IComponentRuntimeParameters? ParametersRuntime { get; } = new MosFetRuntimeParameters();
    public string Type { get; set; }

    public string Value { get; set; }
}
