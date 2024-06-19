using System.Collections.Generic;
using ACDCs.CircuitRenderer.Definitions;

namespace ACDCs.CircuitRenderer.Interfaces;

public interface IDrawInstruction
{
    List<Coordinate> Coordinates { get; }
    Coordinate Position { get; set; }
    Color? StrokeColor { get; set; }
}
