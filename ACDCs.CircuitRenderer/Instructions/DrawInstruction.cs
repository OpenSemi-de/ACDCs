using System;
using ACDCs.CircuitRenderer.Definitions;
using ACDCs.CircuitRenderer.Interfaces;

namespace ACDCs.CircuitRenderer.Instructions;

public class DrawInstruction : IDrawInstruction
{
    public DrawInstruction(Type type)
    {
        Type = type.Name;
    }

    //public List<Color> Colors { get; } = new();
    public Coordinate Position { get; set; }

    //public List<Coordinate> Coordinates { get; set; } = new();
    public Color? StrokeColor { get; set; }

    public string Type { get; }
}
