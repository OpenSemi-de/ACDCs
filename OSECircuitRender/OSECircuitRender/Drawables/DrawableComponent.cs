using System;
using Newtonsoft.Json;
using OSECircuitRender.Definitions;
using OSECircuitRender.Instructions;
using OSECircuitRender.Interfaces;
using OSECircuitRender.Items;

namespace OSECircuitRender.Drawables;

public class DrawableComponent : IDrawableComponent
{
    public DrawableComponent(Type type)
    {
        Type = type.Name;
    }

    [JsonIgnore] public IWorksheetItem BackRef { get; internal set; }

    public Guid ComponentGuid { get; set; } = Guid.NewGuid();
    public DrawablePinList DrawablePins { get; set; } = new();
    public DrawInstructionsList DrawInstructions { get; set; } = new();
    public Coordinate Position { get; set; } = new(0, 0, 0);
    public string RefName => BackRef == null ? "" : BackRef.RefName;
    public float Rotation { get; set; }
    public Coordinate Size { get; set; } = new(1, 1, 0);
    public string Type { get; }

    public void SetPosition(float x, float y)
    {
        Position.X = x;
        Position.Y = y;
    }

    public void SetRef(IWorksheetItem backRef)
    {
        BackRef = backRef;
        DrawablePins.ForEach(pin => pin.SetRef(backRef));
    }

    public void SetSize(int width, int height)
    {
        Size.X = width;
        Size.Y = height;
    }
}