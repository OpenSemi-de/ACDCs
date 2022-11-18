using System;
using OSECircuitRender.Definitions;
using OSECircuitRender.Interfaces;

namespace OSECircuitRender.Instructions;

public class DrawInstruction : IDrawInstruction
{
    public DrawInstruction(Type type)
    {
        Type = type.Name;
    }

    //public List<Color> Colors { get; } = new();
    public Coordinate Position { get; set; }

    //public List<Coordinate> Coordinates { get; set; } = new();
    public Color StrokeColor { get; set; }

    public string Type { get; }
}