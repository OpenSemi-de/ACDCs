namespace ACDCs.CircuitRenderer.Instructions
{
    using System.Collections.Generic;
    using Definitions;

    public sealed class PathPart
    {
        public List<Coordinate> Coordinates { get; }

        public PathPartType Type { get; }

        public PathPart(PathPartType type, List<Coordinate> coordinates)
        {
            Coordinates = coordinates;
            Type = type;
        }
    }
}