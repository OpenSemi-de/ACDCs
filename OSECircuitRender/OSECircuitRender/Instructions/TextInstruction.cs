using OSECircuitRender.Definitions;

namespace OSECircuitRender.Instructions
{
    public sealed class TextInstruction : DrawInstruction
    {
        public string Text;
        public float Orientation;
        public float Size;

        public TextInstruction(string text, float orientation, float size, float x, float y) : base(typeof(TextInstruction))
        {
            Coordinates.Add(new Coordinate(x, y, 0));
            Colors.Add(new Color(0, 0, 0));
            Text = text;
            Orientation = orientation;
            Size = size;
        }
    }
}