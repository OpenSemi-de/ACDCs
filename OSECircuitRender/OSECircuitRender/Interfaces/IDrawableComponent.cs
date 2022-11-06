using OSECircuitRender.Definitions;
using OSECircuitRender.Instructions;
using OSECircuitRender.Items;

namespace OSECircuitRender.Interfaces
{
    public interface IDrawableComponent
    {
        DrawInstructionsList DrawInstructions { get; set; }
        DrawablePinList DrawablePins { get; set; }
        Coordinate Position { get; set; }
        Coordinate Size { get; set; }

        float Rotation { get; set; }
    }
}