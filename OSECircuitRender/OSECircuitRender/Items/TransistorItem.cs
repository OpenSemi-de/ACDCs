using OSECircuitRender.Drawables;

namespace OSECircuitRender.Items;

public class TransistorItem : WorksheetItem
{
    public TransistorItem()
    {
        DrawableComponent = new TransistorDrawable(this, DefaultType, 1, 1);
    }

    public TransistorDrawableType DefaultType { get; set; }

    public new static bool IsInsertable { get; set; } = false;

    public TransistorItem(TransistorDrawableType type, float x, float y)
    {
        DrawableComponent = new TransistorDrawable(this, type, x, y);
        Type = type;
    }

    public TransistorItem(TransistorDrawableType type)
    {
        DrawableComponent = new TransistorDrawable(this, type, 1, 1);
        Type = type;
    }

    public TransistorDrawableType Type { get; set; }
}