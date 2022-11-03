namespace OSECircuitRender
{
    public sealed class DrawCoordinate
    {
        public DrawCoordinate(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.y = y;
        }

        public DrawCoordinate(DrawCoordinate position)
        {
            x = position.x;
            y = position.y;
            z = position.z;
        }

        public DrawCoordinate()
        {
        }

        public float x;
        public float y;
        public float z;
    }
}