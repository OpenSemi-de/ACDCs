using OSECircuitRender.Drawables;

namespace OSECircuitRender.Items.Capacitors;

public  class CapacitorItem : WorksheetItem
{
    public CapacitorItem()
    {
        DrawableComponent = new CapacitorDrawable(this, DefaultValue, DefaultType, 0, 0);
    }
    
    public CapacitorDrawableType DefaultType { get; set; }
    
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
        DrawableComponent = new CapacitorDrawable(this, value, type, 0, 0);
    }

    public string Value { get; set; }
    public CapacitorDrawableType Type { get; }
}