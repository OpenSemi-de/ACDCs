using System;
using OSECircuitRender.Drawables;
using OSECircuitRender.Interfaces;

namespace OSECircuitRender.Items;

public class WorksheetItem : IWorksheetItem
{
    public static bool IsInsertable { get; set; } = false;
    public string DefaultValue { get; set; }
    public IDrawableComponent DrawableComponent { get; set; } = new DrawableComponent(typeof(DrawableComponent));
    public int Height => Convert.ToInt32(DrawableComponent.Size.Y);
    public Guid ItemGuid { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public DrawablePinList Pins { get; set; } = new();
    public string RefName { get; set; } = string.Empty;

    public float Rotation
    {
        get => DrawableComponent.Rotation;
        set => DrawableComponent.Rotation = value;
    }

    public int Width => Convert.ToInt32(DrawableComponent.Size.X);
    public int X => Convert.ToInt32(DrawableComponent.Position.X);
    public int Y => Convert.ToInt32(DrawableComponent.Position.Y);
}