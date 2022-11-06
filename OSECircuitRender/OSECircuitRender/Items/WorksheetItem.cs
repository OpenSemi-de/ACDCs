using System;
using OSECircuitRender.Interfaces;

namespace OSECircuitRender.Items;

public class WorksheetItem : IWorksheetItem
{
    public Guid ItemGuid { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    public IDrawableComponent DrawableComponent { get; set; }
    public float X { get; set; }
    public float Y { get; set; }

    public float Rotation
    {
        get => DrawableComponent.Rotation;
        set => DrawableComponent.Rotation = value;
    }

    public string RefName { get; set; }
}