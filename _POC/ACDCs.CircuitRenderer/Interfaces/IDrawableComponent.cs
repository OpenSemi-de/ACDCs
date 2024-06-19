using System;
using ACDCs.CircuitRenderer.Definitions;
using ACDCs.CircuitRenderer.Drawables;
using ACDCs.CircuitRenderer.Instructions;
using ACDCs.CircuitRenderer.Sheet;

namespace ACDCs.CircuitRenderer.Interfaces;

public interface IDrawableComponent
{
    Guid ComponentGuid { get; set; }
    DrawablePinList DrawablePins { get; set; }
    DrawInstructionsList DrawInstructions { get; set; }
    bool IsMirrored { get; set; }
    bool IsMirroringDone { get; set; }
    Coordinate Position { get; set; }
    float Rotation { get; set; }
    Coordinate Size { get; set; }
    string Value { get; set; }
    Worksheet? Worksheet { get; set; }
}
