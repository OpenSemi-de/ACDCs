namespace OSEData.OSE.Components
{
    public interface IElectronicComponent
    {
        public IComponentParameters ParametersModel { get; }
        public IComponentRuntimeParameters ParametersRuntime { get; }
    }
}