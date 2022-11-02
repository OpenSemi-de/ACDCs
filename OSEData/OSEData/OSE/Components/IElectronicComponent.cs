namespace OSEData.OSE.Components
{
    public interface IElectronicComponent
    {
        public IComponentRuntimeParameters ParametersRuntime { get; }
        public IComponentParameters ParametersModel { get; }
    }
}