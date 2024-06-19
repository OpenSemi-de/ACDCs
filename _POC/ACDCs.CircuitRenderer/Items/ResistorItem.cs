using ACDCs.CircuitRenderer.Drawables;
using ACDCs.Data.ACDCs.Components.Resistor;
using Newtonsoft.Json;

namespace ACDCs.CircuitRenderer.Items;

public sealed class ResistorItem : WorksheetItem
{
    public override string DefaultValue => "10k";

    public override bool IsInsertable => true;

    [JsonConstructor]
    public ResistorItem(string value = "")
    {
        DrawableComponent = new ResistorDrawable();
        Value = value;
    }

    public ResistorItem()
    {
        Model = new Resistor();
        DrawableComponent = new ResistorDrawable(this, DefaultValue, 1, 1);
        Value = DefaultValue;
    }

    public ResistorItem(string value, float x, float y)
    {
        Model = new Resistor();
        DrawableComponent = new ResistorDrawable(this, value, x, y);
        Value = value;
    }
}
