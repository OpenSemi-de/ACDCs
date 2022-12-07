using OSECircuitRender.Definitions;
using OSECircuitRender.Instructions;
using OSECircuitRender.Items;
using OSECircuitRender.Sheet;
using System;

namespace OSECircuitRender.Interfaces;

public interface IDrawableComponent
{
    Guid ComponentGuid { get; set; }
    DrawablePinList DrawablePins { get; set; }
    DrawInstructionsList DrawInstructions { get; set; }
    Coordinate Position { get; set; }
    float Rotation { get; set; }
    Coordinate Size { get; set; }
    Worksheet? Worksheet { get; set; }
}