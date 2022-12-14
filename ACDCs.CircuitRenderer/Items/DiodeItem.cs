using ACDCs.CircuitRenderer.Drawables;

namespace ACDCs.CircuitRenderer.Items;

public sealed class DiodeItem : WorksheetItem
{
    public override string DefaultValue => "0.7";

    public override bool IsInsertable => true;

    public DiodeItem()
    {
        DrawableComponent = new DiodeDrawable(this, DefaultValue, 1, 1);
        Value = DefaultValue;
    }

    public DiodeItem(string value, float x, float y)
    {
        DrawableComponent = new DiodeDrawable(this, value, x, y);
        Value = value;
    }

    public DiodeItem(string value)
    {
        DrawableComponent = new DiodeDrawable(this, value, 1, 1);
        Value = value;
    }
}
