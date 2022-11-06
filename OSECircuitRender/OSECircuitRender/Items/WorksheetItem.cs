using System;
using OSECircuitRender.Interfaces;

namespace OSECircuitRender.Items;

public class WorksheetItem : IWorksheetItem
{
    public Guid ItemGuid { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public IDrawableComponent DrawableComponent { get; set; }
    public float X { get; set; } = 0f;
    public float Y { get; set; } = 0f;

    public float Rotation
    {
        get => DrawableComponent.Rotation;
        set => DrawableComponent.Rotation = value;
    }

    public string RefName { get; set; } = string.Empty;

    public DrawablePinList Pins { get; set; } = new();
}