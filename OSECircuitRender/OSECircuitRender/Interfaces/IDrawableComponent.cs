using OSECircuitRender.Definitions;
using OSECircuitRender.Instructions;
using OSECircuitRender.Items;

namespace OSECircuitRender.Interfaces
{
    public interface IDrawableComponent
    {
        DrawInstructionsList DrawInstructions { get; }
        DrawablePinList DrawablePins { get; }
        Coordinate Position { get; }
        Coordinate Size { get; }
    }
}