using System;
using OSECircuitRender.Drawables;
using OSECircuitRender.Interfaces;

namespace OSECircuitRender.Items;

public class WorksheetItem : IWorksheetItem
{
    public Guid ItemGuid { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public IDrawableComponent DrawableComponent { get; set; } = new DrawableComponent(typeof(DrawableComponent));

    public int X => Convert.ToInt32(DrawableComponent.Position.X);
    public int Y => Convert.ToInt32(DrawableComponent.Position.Y);

    public float Rotation
    {
        get => DrawableComponent.Rotation;
        set => DrawableComponent.Rotation = value;
    }

    public string RefName { get; set; } = string.Empty;

    public DrawablePinList Pins { get; set; } = new();
    public int Width => Convert.ToInt32(DrawableComponent.Size.X);
    public int Height => Convert.ToInt32(DrawableComponent.Size.Y);
}