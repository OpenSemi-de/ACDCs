using OSECircuitRender.Definitions;

namespace OSECircuitRender.Instructions
{
    public sealed class BoxInstruction : DrawInstruction
    {
        public BoxInstruction(float x1, float y1, float width, float height) : base(typeof(BoxInstruction))
        {
            Colors.Add(new Color(255, 255, 255));
            Coordinates.Add(new Coordinate(x1, y1, 0));
            Coordinates.Add(new Coordinate(width, height, 0));
            Colors.Add(new Color(0, 0, 0));
        }
    }
}