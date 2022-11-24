using OSECircuitRender.Drawables;

namespace OSECircuitRender.Items;

public class CapacitorItem : WorksheetItem
{
    public CapacitorItem()
    {
        DrawableComponent = new CapacitorDrawable(this, DefaultValue, DefaultType, 1, 1);
        Value = DefaultValue;
    }

    public CapacitorItem(string value, CapacitorDrawableType type, float x, float y)
    {
        DrawableComponent = new CapacitorDrawable(this, value, type, x, y);
        Value = value;
        Type = type;
    }

    public CapacitorItem(string value, CapacitorDrawableType type)
    {
        Value = value;
        Type = type;
        DrawableComponent = new CapacitorDrawable(this, value, type, 1, 1);
    }

    public CapacitorDrawableType DefaultType { get; set; }
    public CapacitorDrawableType Type { get; }

}