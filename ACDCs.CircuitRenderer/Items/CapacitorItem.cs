using ACDCs.CircuitRenderer.Drawables;
using ACDCs.Data.ACDCs.Components;
using ACDCs.Data.ACDCs.Components.Capacitor;

namespace ACDCs.CircuitRenderer.Items;

public class CapacitorItem : WorksheetItem
{
    public CapacitorDrawableType DefaultType { get; set; } = CapacitorDrawableType.Standard;

    public override string DefaultValue => "10u";

    public override bool IsInsertable => false;

    public CapacitorDrawableType Type { get; }

    public CapacitorItem()
    {
        DrawableComponent = new CapacitorDrawable(this, DefaultValue, DefaultType, 1, 1);
        Value = DefaultValue;
        Model = new Capacitor();
    }

    public CapacitorItem(string value, CapacitorDrawableType type, float x, float y)
    {
        DrawableComponent = new CapacitorDrawable(this, value, type, x, y);
        Value = value;
        Type = type;
        Model = new Capacitor();
    }

    public CapacitorItem(string value, CapacitorDrawableType type)
    {
        Value = value;
        Type = type;
        DrawableComponent = new CapacitorDrawable(this, value, type, 1, 1);
        Model = new Capacitor();
    }
}
