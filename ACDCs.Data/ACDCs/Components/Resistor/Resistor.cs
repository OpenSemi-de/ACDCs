namespace ACDCs.Data.ACDCs.Components.Resistor;

public class Resistor : ResistorParameters, IElectronicComponent
{
    private string _value = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public IComponentParameters ParametersModel => new ResistorParameters();
    public IComponentRuntimeParameters ParametersRuntime => new ResistorRuntimeParameters();
    public string Type { get; set; } = string.Empty;

    public string Value
    {
        get => _value;

        set
        {
            _value = value;
            ((ResistorRuntimeParameters)ParametersRuntime).Resistance = Convert.ToDouble(value);
        }
    }
}
