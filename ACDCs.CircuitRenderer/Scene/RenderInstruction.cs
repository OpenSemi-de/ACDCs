using ACDCs.CircuitRenderer.Definitions;

namespace ACDCs.CircuitRenderer.Scene
{
    public class RenderInstruction
    {
        public float Zoom { get; set; }
        public float BaseGridSize { get; set; }
        public Color? ForegroundColor { get; set; }
        public Color? BackgroundColor { get; set; }
        public Coordinate DrawPos { get; set; } = new();
        public Coordinate DrawSize { get; set; } = new();
        public float FontSize { get; set; }
    }
}
