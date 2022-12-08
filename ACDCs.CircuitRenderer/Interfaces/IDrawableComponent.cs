using System;
using ACDCs.CircuitRenderer.Definitions;
using ACDCs.CircuitRenderer.Instructions;
using ACDCs.CircuitRenderer.Items;
using ACDCs.CircuitRenderer.Sheet;

namespace ACDCs.CircuitRenderer.Interfaces;

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
