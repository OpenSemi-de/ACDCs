namespace OSECircuitRender
{
    public sealed class Box : DrawInstruction
    {
        public Box(float x1, float y1, float width, float height) : base(typeof(Box))
        {
            Colors.Add(new Color(255, 255, 255));
            Coordinates.Add(new DrawCoordinate(x1, y1, 0));
            Coordinates.Add(new DrawCoordinate(width, height, 0));
            Colors.Add(new Color(0, 0, 0));
        }
    }
}