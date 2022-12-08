using ACDCs.CircuitRenderer.Drawables;

namespace ACDCs.CircuitRenderer.Items;

public sealed class ResistorItem : WorksheetItem
{
    public ResistorItem()
    {
        DrawableComponent = new ResistorDrawable(this, DefaultValue, 1, 1);
        Value = DefaultValue;
    }

    public ResistorItem(string value, float x, float y)
    {
        DrawableComponent = new ResistorDrawable(this, value, x, y);
        Value = value;
    }

    public static new string DefaultValue { get; set; } = "10k";

    public static new bool IsInsertable { get; set; } = true;
}
