namespace ACDCs.Data.ACDCs.Components;

public interface IElectronicComponent
{
    public IComponentParameters ParametersModel { get; }
    public IComponentRuntimeParameters ParametersRuntime { get; }
}
