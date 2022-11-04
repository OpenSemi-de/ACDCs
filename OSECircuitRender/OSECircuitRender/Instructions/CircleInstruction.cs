using OSECircuitRender.Definitions;

namespace OSECircuitRender.Instructions
{
    public sealed class CircleInstruction : DrawInstruction
    {
        public Coordinate Size { get; set; }
        public Color FillColor { get; set; }

        public CircleInstruction(float centerX, float centerY, float width, float height) : base(typeof(CircleInstruction))
        {
            Position = new(centerX, centerY, 0);
            Size = new(width, height, 0);
            StrokeColor = new Color(0, 0, 0);
            FillColor = new Color(255, 255, 255);
        }
    }
}