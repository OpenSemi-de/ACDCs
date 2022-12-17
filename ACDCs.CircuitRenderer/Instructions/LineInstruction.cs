using ACDCs.CircuitRenderer.Definitions;

namespace ACDCs.CircuitRenderer.Instructions;

public sealed class LineInstruction : DrawInstruction
{
    public LineInstruction(float x1, float y1, float x2, float y2, int strokeWidth = 1) : base(typeof(LineInstruction))
    {
        Position = new Coordinate(x1, y1, 0);
        End = new Coordinate(x2, y2, 0);
        StrokeColor = new Color(0, 0, 0);
        StrokeWidth = strokeWidth;
        Coordinates.Add(Position);
        Coordinates.Add(End);
    }

    public Coordinate End { get; set; }

    public float StrokeWidth { get; set; }
}
