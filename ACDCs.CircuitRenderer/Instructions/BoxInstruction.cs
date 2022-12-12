using ACDCs.CircuitRenderer.Definitions;

namespace ACDCs.CircuitRenderer.Instructions;

public sealed class BoxInstruction : DrawInstruction
{
    public BoxInstruction(float x1, float y1, float width, float height, Color? fillColor = null) : base(
        typeof(BoxInstruction))
    {
        Position = new Coordinate(x1, y1, 0);
        Size = new Coordinate(width, height, 0);
        StrokeColor = new Color(0, 0, 0);
        FillColor = fillColor ?? new Color(255, 255, 255);
    }

    public Color? FillColor { get; set; }
    public Coordinate Size { get; set; }
}
