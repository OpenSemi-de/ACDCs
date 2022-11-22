using System.ComponentModel;
using OSECircuitRender.Drawables;

namespace OSECircuitRender.Items;

public sealed class InductorItem : WorksheetItem
{
    public string Value { get; }

    public InductorItem()
    {
        DrawableComponent = new InductorDrawable(this, DefaultValue,0,0 );
    }

    public string DefaultValue { get; set; } = "1m";

    public new static bool IsInsertable { get; set; } = true;


    public InductorItem(string value, float x, float y)
    {
        Value = value;
        DrawableComponent = new InductorDrawable(this, value, x, y);
    }

    public InductorItem(string value)
    {
        Value = value;
        DrawableComponent = new InductorDrawable(this, value, 0, 0);
    }
}