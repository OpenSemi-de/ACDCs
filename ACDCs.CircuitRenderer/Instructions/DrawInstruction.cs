using System;
using System.Collections.Generic;
using ACDCs.CircuitRenderer.Definitions;
using ACDCs.CircuitRenderer.Interfaces;

namespace ACDCs.CircuitRenderer.Instructions;

public class DrawInstruction : IDrawInstruction
{
    public DrawInstruction(Type type)
    {
        Type = type.Name;
        Position = new();
    }

    public Coordinate Position { get; set; }

    public List<Coordinate> Coordinates { get; } = new();

    public Color? StrokeColor { get; set; }

    public string Type { get; }
}
