using ACDCs.CircuitRenderer.Drawables;
using ACDCs.Data.ACDCs.Components.Source;

namespace ACDCs.CircuitRenderer.Items;

public class SourceItem : WorksheetItem
{
    public override string DefaultValue => "DC 5v";

    public override bool IsInsertable => false;

    public SourceDrawableType Type
    {
        get => ((SourceDrawable)DrawableComponent).SourceType;
        set => ((SourceDrawable)DrawableComponent).SourceType = value;
    }

    public SourceItem()
    {
        DrawableComponent = new SourceDrawable(this, DefaultValue, SourceDrawableType.Voltage, 1, 1);
        Value = DefaultValue;
        Model = new Source();
    }

    public SourceItem(SourceDrawableType sourceDrawableType)
    {
        DrawableComponent = new SourceDrawable(this, DefaultValue, sourceDrawableType, 1, 1);
        Value = DefaultValue;
        Model = new Source();
    }

    public SourceItem(string value, SourceDrawableType type, float x, float y)
    {
        DrawableComponent = new SourceDrawable(this, value, type, x, y);
        Value = value;
        Model = new Source();
    }
}
