using ACDCs.CircuitRenderer.Definitions;

namespace ACDCs.CircuitRenderer.Instructions;

public sealed class TextInstruction : DrawInstruction
{
    public float Orientation;
    public float Size;
    public string Text;

    public TextInstruction(string text, float orientation, float size, float x, float y) : base(typeof(TextInstruction))
    {
        Position = new Coordinate(x, y, 0);
        StrokeColor = new Color(0, 0, 0);
        Text = text;
        Orientation = orientation;
        Size = size;
    }
}
