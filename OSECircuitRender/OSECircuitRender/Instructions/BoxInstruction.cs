using System.Security.Cryptography.X509Certificates;
using Microsoft.Maui.Graphics;
using OSECircuitRender.Definitions;
using Color = OSECircuitRender.Definitions.Color;

namespace OSECircuitRender.Instructions
{
    public sealed class BoxInstruction : DrawInstruction
    {
        public Coordinate Size { get; set; }

        public BoxInstruction(float x1, float y1, float width, float height) : base(typeof(BoxInstruction))
        {
            Position = new(x1, y1, 0);
            Size = new Coordinate(width, height, 0);
            StrokeColor = new Color(0, 0, 0);
            FillColor = new Color(255, 255, 255);
        }

        public Color FillColor { get; set; }
    }
}