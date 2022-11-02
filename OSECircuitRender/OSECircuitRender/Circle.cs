namespace OSECircuitRender
{
    public sealed class Circle : DrawInstruction
    {
        public Circle(float centerX, float centerY, float width, float height) : base(typeof(Circle))
        {
            Coordinates.Add(new DrawCoordinate(centerX, centerY, 0));
            Coordinates.Add(new DrawCoordinate(width, height, 0));
            Colors.Add(new Color(0, 0, 0));
            Colors.Add(new Color(255, 255, 255));
        }
    }
}