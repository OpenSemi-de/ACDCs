namespace OSECircuitRender
{
    public sealed class Text : DrawInstruction
    {
        public string text;
        public float orientation;
        public float size;

        public Text(string text, float orientation, float size, float x, float y) : base(typeof(Text))
        {
            Coordinates.Add(new DrawCoordinate(x, y, 0));
            Colors.Add(new Color(0, 0, 0));
            this.text = text;
            this.orientation = orientation;
            this.size = size;
        }
    }
}