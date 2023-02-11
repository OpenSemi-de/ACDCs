using System.Collections.Generic;
using ACDCs.CircuitRenderer.Definitions;

namespace ACDCs.CircuitRenderer.Instructions;

public sealed class PathInstruction : DrawInstruction
{
    private readonly List<PathPart> _pathParts;

    public float Height { get; set; }

    public float Width { get; set; }

    public PathInstruction(string svgPath) : base(typeof(PathInstruction))
    {
        StrokeColor = new Color(0, 0, 0);
        PathReader pr = new(svgPath);
        _pathParts = pr.GetPathParts();
        Width = pr.GetWidth();
        Height = pr.GetHeight();
        Coordinates.Add(Position);
    }

    public List<PathPart> GetParts()
    {
        return _pathParts;
    }
}
