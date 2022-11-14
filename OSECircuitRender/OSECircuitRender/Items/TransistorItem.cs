using OSECircuitRender.Drawables;

namespace OSECircuitRender.Items;

public sealed class TransistorItem : WorksheetItem
{
    public TransistorItem()
    {
        DrawableComponent = new TransistorDrawable(this);
    }

    public TransistorItem(TransistorDrawableType type, float x, float y)
    {
        DrawableComponent = new TransistorDrawable(this, type, x, y);
        Type = type;
    }

    public TransistorDrawableType Type { get; set; }
}