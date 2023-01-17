namespace ACDCs.Data.ACDCs.Components.Resistor;

public class Resistor : ResistorParameters, IElectronicComponent
{
    private string _value;
    public string Model { get; set; }
    public string Name { get; set; }
    public IComponentParameters ParametersModel => new ResistorParameters();
    public IComponentRuntimeParameters ParametersRuntime => new ResistorRuntimeParameters();
    public string Type { get; set; }

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
