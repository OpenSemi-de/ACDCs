using ACDCs.CircuitRenderer.Definitions;

namespace ACDCs.CircuitRenderer.Interfaces;

public interface IDrawInstruction
{
    Coordinate Position { get; set; }
    Color? StrokeColor { get; set; }
}
