#nullable enable

using System;
using ACDCs.CircuitRenderer.Drawables;
using ACDCs.CircuitRenderer.Interfaces;
using Newtonsoft.Json;

namespace ACDCs.CircuitRenderer.Items;

public class WorksheetItem : IWorksheetItem
{
    private string? _value;

    public virtual string DefaultValue { get; set; } = "";

    [JsonIgnore] public IDrawableComponent DrawableComponent { get; set; }

    public int Height
    {
        get => Convert.ToInt32(DrawableComponent.Size.Y);
        set => DrawableComponent.Size.Y = value;
    }

    public virtual bool IsInsertable { get; set; }

    public bool IsMirrored
    {
        get => DrawableComponent.IsMirrored;
        set => DrawableComponent.IsMirrored = value;
    }

    public Guid ItemGuid { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = string.Empty;

    [JsonIgnore] public DrawablePinList Pins { get; set; } = new();

    public string RefName { get; set; } = string.Empty;

    public float Rotation
    {
        get => DrawableComponent.Rotation;
        set => DrawableComponent.Rotation = value;
    }

    public string TypeName => GetType().Name.Replace("Item", "");

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

    public WorksheetItem()
    {
        DrawableComponent = new DrawableComponent(typeof(DrawableComponent), this);
    }
}
