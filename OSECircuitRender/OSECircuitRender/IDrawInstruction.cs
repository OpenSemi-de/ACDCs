using System.Collections.Generic;

namespace OSECircuitRender
{
    public interface IDrawInstruction
    {
        List<DrawCoordinate> Coordinates { get; }
        List<Color> Colors { get; }
    }
}