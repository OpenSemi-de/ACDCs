using System;
using OSECircuitRender.Drawables;
using OSECircuitRender.Interfaces;

namespace OSECircuitRender.Items;

public sealed class TransistorItem : IWorksheetItem
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

    public string RefName { get; set; }
    public IDrawableComponent DrawableComponent { get; set; }
}