using ACDCs.CircuitRenderer.Drawables;

namespace ACDCs.CircuitRenderer.Items;

public class TransistorItem : WorksheetItem
{
    public TransistorItem()
    {
        DrawableComponent = new TransistorDrawable(this, DefaultType, 1, 1);
        Value = DefaultType.ToString();
    }

    public TransistorItem(TransistorDrawableType type, float x, float y)
    {
        DrawableComponent = new TransistorDrawable(this, type, x, y);
        Type = type;
        Value = type.ToString();
    }

    public TransistorItem(TransistorDrawableType type)
    {
        DrawableComponent = new TransistorDrawable(this, type, 1, 1);
        Type = type;
        Value = type.ToString();
    }

    public new static bool IsInsertable { get; set; } = false;
    public TransistorDrawableType DefaultType { get; set; }
    public TransistorDrawableType Type { get; set; }
}
