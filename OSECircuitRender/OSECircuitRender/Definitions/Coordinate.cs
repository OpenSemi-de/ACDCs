namespace OSECircuitRender.Definitions
{
    public sealed class Coordinate
    {
        public Coordinate(float x, float y, float z)
        {
            X = x;
            Y = y;
            Y = y;
        }

        public Coordinate(Coordinate coordinate)
        {
            X = coordinate.X;
            Y = coordinate.Y;
            Z = coordinate.Z;
        }

        public Coordinate()
        {
        }

        public float X;
        public float Y;
        public float Z;
    }
}