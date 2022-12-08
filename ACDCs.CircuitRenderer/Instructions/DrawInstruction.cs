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

    public Coordinate Position { get; set; }

    public Color? StrokeColor { get; set; }

    public string Type { get; }
}
