using System.Collections.Generic;
using OSECircuitRender.Definitions;

namespace OSECircuitRender.Interfaces
{
    public interface IDrawInstruction
    {
        List<Coordinate> Coordinates { get; }
        List<Color> Colors { get; }
    }
}