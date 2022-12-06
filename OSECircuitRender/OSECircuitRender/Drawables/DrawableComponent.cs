using Newtonsoft.Json;
using OSECircuitRender.Definitions;
using OSECircuitRender.Instructions;
using OSECircuitRender.Interfaces;
using OSECircuitRender.Items;
using System;
using System.Linq;
using OSECircuitRender.Sheet;

namespace OSECircuitRender.Drawables;

public class DrawableComponent : IDrawableComponent, IHaveAParent
{
    public DrawableComponent(Type type, IWorksheetItem? parentItem)
    {
        ParentItem = parentItem;
        Type = type.Name;
    }

    public Guid ComponentGuid { get; set; } = Guid.NewGuid();
    public DrawablePinList DrawablePins { get; set; } = new();
    public DrawInstructionsList DrawInstructions { get; set; } = new();
    public Coordinate Position { get; set; } = new(0, 0, 0);
    public string RefName => ParentItem == null ? "" : ParentItem.RefName;
    public float Rotation { get; set; }
    public Coordinate Size { get; set; } = new(1, 1, 0);
    public Worksheet? Worksheet { get; set; }
    public string Type { get; }

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

    public IWorksheetItem? ParentItem { get; set; }
}