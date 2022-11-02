namespace OSECircuitRender
{
    public sealed class Line : DrawInstruction
    {
        public Line(float x1, float y1, float x2, float y2) : base(typeof(Line))
        {
            Coordinates.Add(new DrawCoordinate(x1, y1, 0));
            Coordinates.Add(new DrawCoordinate(x2, y2, 0));
            Colors.Add(new Color(0, 0, 0));
        }
    }
}