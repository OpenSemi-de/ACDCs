using Microsoft.Maui;
using OSECircuitRender.Definitions;

namespace OSECircuitRender.Instructions
{
    public sealed class LineInstruction : DrawInstruction
    {
        public Coordinate End { get; set; }

        public LineInstruction(float x1, float y1, float x2, float y2) : base(typeof(LineInstruction))
        {
            Position = new(x1, y1, 0);
            End = new(x2, y2, 0);
            StrokeColor = new Color(0, 0, 0);
        }
    }
}