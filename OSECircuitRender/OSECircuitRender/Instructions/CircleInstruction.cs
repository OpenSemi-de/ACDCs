using OSECircuitRender.Definitions;

namespace OSECircuitRender.Instructions
{
    public sealed class CircleInstruction : DrawInstruction
    {
        public CircleInstruction(float centerX, float centerY, float width, float height) : base(typeof(CircleInstruction))
        {
            Coordinates.Add(new Coordinate(centerX, centerY, 0));
            Coordinates.Add(new Coordinate(width, height, 0));
            Colors.Add(new Color(0, 0, 0));
            Colors.Add(new Color(255, 255, 255));
        }
    }
}