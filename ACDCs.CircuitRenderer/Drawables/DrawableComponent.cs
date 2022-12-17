using System;
using ACDCs.CircuitRenderer.Definitions;
using ACDCs.CircuitRenderer.Instructions;
using ACDCs.CircuitRenderer.Interfaces;
using ACDCs.CircuitRenderer.Sheet;

namespace ACDCs.CircuitRenderer.Drawables;

public class DrawableComponent : IDrawableComponent, IHaveAParent
{
    public DrawableComponent(Type type, IWorksheetItem parentItem)
    {
        ParentItem = parentItem;
        Type = type.Name;
        Value = "";
    }

    public Guid ComponentGuid { get; set; } = Guid.NewGuid();
    public DrawablePinList DrawablePins { get; set; } = new();
    public DrawInstructionsList DrawInstructions { get; set; } = new();
    public IWorksheetItem ParentItem { get; set; }
    public Coordinate Position { get; set; } = new(0, 0, 0);
    public string RefName => ParentItem.RefName;
    public float Rotation { get; set; }
    public Coordinate Size { get; set; } = new(1, 1, 0);
    public string Type { get; }
    public string Value { get; set; }
    public Worksheet? Worksheet { get; set; }
    public bool IsMirrored { get; set; }
    public bool IsMirroringDone { get; set; }

    public void SetPosition(float x, float y)
    {
        Position.X = x;
        Position.Y = y;
    }

    public void SetSize(int width, int height)
    {
        Size.X = width;
        Size.Y = height;
    }
}
