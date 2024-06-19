using ACDCs.CircuitRenderer.Definitions;

namespace ACDCs.CircuitRenderer.Instructions;

public sealed class CircleInstruction : DrawInstruction
{
    public Color FillColor { get; set; }

    public Coordinate Size { get; set; }

    public CircleInstruction(float centerX, float centerY, float width, float height) : base(typeof(CircleInstruction))
    {
        Position = new Coordinate(centerX, centerY, 0);
        Size = new Coordinate(width, height, 0);
        StrokeColor = new Color(0, 0, 0);
        FillColor = new Color(255, 255, 255);
        Coordinates.Add(Position);
    }
}
