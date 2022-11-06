using System;
using OSECircuitRender.Drawables;
using OSECircuitRender.Interfaces;

namespace OSECircuitRender.Items;

public sealed class TransistorItem : WorksheetItem
{
    public TransistorDrawableType Type { get; set; }

    public TransistorItem()
    {
        DrawableComponent = new TransistorDrawable(this);
    }

    public TransistorItem(TransistorDrawableType type, float x, float y)
    {
        DrawableComponent = new TransistorDrawable(this, type, x, y);
        Type = type;
    }
}