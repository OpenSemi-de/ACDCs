using OSECircuitRender.Definitions;

namespace OSECircuitRender.Interfaces
{
    public interface IDrawInstruction
    {
        Coordinate Position { get; set; }
        Color StrokeColor { get; set; }
    }
}