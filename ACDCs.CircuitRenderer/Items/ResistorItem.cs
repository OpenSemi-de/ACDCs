using ACDCs.CircuitRenderer.Drawables;
using ACDCs.Data.ACDCs.Components.Resistor;

namespace ACDCs.CircuitRenderer.Items;

public sealed class ResistorItem : WorksheetItem
{
    public override string DefaultValue => "10k";

    public override bool IsInsertable => true;

    public ResistorItem()
    {
        this.Model = new Resistor();
        DrawableComponent = new ResistorDrawable(this, DefaultValue, 1, 1);
        Value = DefaultValue;
    }

    public ResistorItem(string value, float x, float y)
    {
        this.Model = new Resistor();
        DrawableComponent = new ResistorDrawable(this, value, x, y);
        Value = value;
    }
}
