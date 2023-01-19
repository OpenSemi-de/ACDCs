using ACDCs.CircuitRenderer.Drawables;

namespace ACDCs.CircuitRenderer.Items;

public class SourceItem : WorksheetItem
{
    public override string DefaultValue => "5v";

    public override bool IsInsertable => false;

    public SourceItem()
    {
        DrawableComponent = new SourceDrawable(this, DefaultValue, SourceDrawableType.Voltage, 1, 1);
        Value = DefaultValue;
    }

    public SourceItem(SourceDrawableType sourceDrawableType)
    {
        DrawableComponent = new SourceDrawable(this, DefaultValue, sourceDrawableType, 1, 1);
        Value = DefaultValue;
    }

    public SourceItem(string value, SourceDrawableType type, float x, float y)
    {
        DrawableComponent = new SourceDrawable(this, value, type, x, y);
        Value = value;
    }
}
