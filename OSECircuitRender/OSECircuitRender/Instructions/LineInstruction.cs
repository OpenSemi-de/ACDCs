using OSECircuitRender.Definitions;

namespace OSECircuitRender.Instructions
{
    public sealed class LineInstruction : DrawInstruction
    {
        public LineInstruction(float x1, float y1, float x2, float y2) : base(typeof(LineInstruction))
        {
            Coordinates.Add(new Coordinate(x1, y1, 0));
            Coordinates.Add(new Coordinate(x2, y2, 0));
            Colors.Add(new Color(0, 0, 0));
        }

        public float X1 => Coordinates[0].X;
        public float Y1 => Coordinates[0].Y;
        public float X2 => Coordinates[1].X;
        public float Y2 => Coordinates[1].Y;
    }
}