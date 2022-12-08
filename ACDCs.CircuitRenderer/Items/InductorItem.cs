using ACDCs.CircuitRenderer.Drawables;

namespace ACDCs.CircuitRenderer.Items;

public sealed class InductorItem : WorksheetItem
{
    public InductorItem()
    {
        DrawableComponent = new InductorDrawable(this, DefaultValue, 1, 1);
        Value = DefaultValue;
    }

    public InductorItem(string value, float x, float y)
    {
        Value = value;
        DrawableComponent = new InductorDrawable(this, value, x, y);
    }

    public InductorItem(string value)
    {
        Value = value;
        DrawableComponent = new InductorDrawable(this, value, 1, 1);
    }

    public static new bool IsInsertable { get; set; } = true;
    public new string DefaultValue { get; set; } = "1m";
}
