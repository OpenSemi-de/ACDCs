#nullable enable

using OSECircuitRender.Drawables;
using OSECircuitRender.Interfaces;
using System;

namespace OSECircuitRender.Items;

public class WorksheetItem : IWorksheetItem
{
    private string? _value;

    public static string? DefaultValue { get; set; }

    public static bool IsInsertable { get; set; } = false;

    public IDrawableComponent DrawableComponent { get; set; } = new DrawableComponent(typeof(DrawableComponent));

    public int Height
    {
        get => Convert.ToInt32(DrawableComponent.Size.Y);
        set => DrawableComponent.Size.Y = value;
    }

    public Guid ItemGuid { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = string.Empty;

    public DrawablePinList Pins { get; set; } = new();

    public string RefName { get; set; } = string.Empty;

    public float Rotation
    {
        get => DrawableComponent.Rotation;
        set => DrawableComponent.Rotation = value;
    }

    public string TypeName => this.GetType().Name.Replace("Item", "");

    public string? Value
    {
        get => _value ?? DefaultValue;
        set => _value = value ?? DefaultValue;
    }

    public int Width
    {
        get => Convert.ToInt32(DrawableComponent.Size.X);
        set => DrawableComponent.Size.X = value;
    }

    public int X
    {
        get => Convert.ToInt32(DrawableComponent.Position.X);
        set => DrawableComponent.Position.X = value;
    }

    public int Y
    {
        get => Convert.ToInt32(DrawableComponent.Position.Y);
        set => DrawableComponent.Position.Y = value;
    }
}