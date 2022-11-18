using System;
using OSECircuitRender.Items;

namespace OSECircuitRender.Interfaces;

public interface IWorksheetItem
{
    IDrawableComponent DrawableComponent { get; set; }
    int Height { get; }
    public Guid ItemGuid { get; set; }
    public string Name { get; set; }
    public DrawablePinList Pins { get; set; }
    string RefName { get; set; }
    float Rotation { get; set; }
    int Width { get; }
    int X { get; }
    int Y { get; }
}