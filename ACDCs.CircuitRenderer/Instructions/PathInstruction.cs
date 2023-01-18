using System;
using System.Collections.Generic;
using System.Linq;
using ACDCs.CircuitRenderer.Definitions;

namespace ACDCs.CircuitRenderer.Instructions;

public enum PathPartType
{
    M = 0,
    L = 1,
    H = 2,
    V = 3,
    C = 4,
    S = 5,
    Q = 6,
    T = 7,
    A = 8,
    Z = 9
}

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

public sealed class PathReader
{
    private readonly float _height;

    private readonly List<PathPart> _pathParts = new();

    private readonly float _width;

    public PathReader(string svgPath)
    {
        _width = 0;
        _height = 0;
        svgPath += "Z";
        string? buffer = string.Empty;
        string? command = string.Empty;
        foreach (char chrBuffer in svgPath)
            if ((chrBuffer > 64 && chrBuffer < 91) ||
                (chrBuffer > 96 && chrBuffer < 123))
            {
                if (command != "")
                {
                    List<Coordinate> coordinates = new();
                    PathPartType type = (PathPartType)Enum.Parse(typeof(PathPartType), command);
                    List<string> textCoordinates = buffer.Split(" ").ToList();
                    foreach (string? textCoordinate in textCoordinates.Where(s => s != "" && s != " "))
                    {
                        string[]? xypair = textCoordinate.Split(',');
                        Coordinate coordinate = new(
                            float.Parse(xypair[0].Replace(".", ",")),
                            float.Parse(xypair[1].Replace(".", ",")),
                            0
                        );

                        if (coordinate.X > _width)
                            _width = coordinate.X;
                        if (coordinate.Y > _height)
                            _height = coordinate.Y;

                        coordinates.Add(coordinate);
                    }

                    _pathParts.Add(new PathPart(type, coordinates));
                }

                command = chrBuffer.ToString();
                buffer = string.Empty;
            }
            else
            {
                buffer += chrBuffer;
            }
    }

    public float GetHeight()
    {
        return _height;
    }

    public List<PathPart> GetPathParts()
    {
        return _pathParts;
    }

    public float GetWidth()
    {
        return _width;
    }
}
