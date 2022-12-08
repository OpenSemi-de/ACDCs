using System;
using ACDCs.CircuitRenderer.Items;

namespace ACDCs.CircuitRenderer.Interfaces;

public interface IWorksheetItem
{
    static string DefaultValue { get; set; } = "";
    static bool IsInsertable { get; set; }
    IDrawableComponent DrawableComponent { get; set; }
    int Height { get; set; }
    public Guid ItemGuid { get; set; }
    public string Name { get; set; }
    public DrawablePinList Pins { get; set; }
    string RefName { get; set; }
    float Rotation { get; set; }
    int Width { get; set; }
    int X { get; set; }
    int Y { get; set; }
}
