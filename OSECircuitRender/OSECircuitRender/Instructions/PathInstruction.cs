using System;
using System.Collections.Generic;
using System.Linq;
using OSECircuitRender.Definitions;

namespace OSECircuitRender.Instructions
{
    public sealed class PathInstruction : DrawInstruction
    {
        private readonly List<PathPart> _pathParts;

        public float Width { get; set; }
        public float Height { get; set; }

        public PathInstruction(string svgPath) : base(typeof(PathInstruction))
        {
            StrokeColor = new Color(0, 0, 0);
            PathReader pr = new(svgPath);
            _pathParts = pr.GetPathParts();
            Width = pr.GetWidth();
            Height = pr.GetHeight();
        }

        public List<PathPart> GetParts()
        {
            return _pathParts;
        }
    }

    public sealed class PathReader
    {
        private readonly List<PathPart> _pathParts = new();
        private float width;
        private float height;

        public PathReader(string svgPath)
        {
            width = 0;
            height = 0;
            svgPath += "Z";
            string buffer = "";
            string command = "";
            foreach (var chrBuffer in svgPath)
            {
                if (chrBuffer > 64 && chrBuffer < 91 ||
                    chrBuffer > 96 && chrBuffer < 123)
                {
                    if (command != "")
                    {
                        List<Coordinate> coordinates = new();
                        PathPartType type = (PathPartType)Enum.Parse(typeof(PathPartType), command);
                        List<string> textCoordinates = buffer.Split(" ").ToList();
                        foreach (var textCoordinate in textCoordinates.Where(s => s != "" && s != " "))
                        {
                            var xypair = textCoordinate.Split(',');
                            var coordinate = new Coordinate(
                                float.Parse(xypair[0].Replace(".", ",")),
                                float.Parse(xypair[1].Replace(".", ",")),
                                0
                            );

                            if (coordinate.X > width)
                                width = coordinate.X;
                            if (coordinate.Y > height)
                                height = coordinate.Y;

                            coordinates.Add(coordinate);
                        }

                        _pathParts.Add(new PathPart(type, coordinates));
                    }

                    command = chrBuffer.ToString();
                    buffer = "";
                }
                else
                {
                    buffer += chrBuffer;
                }
            }
        }

        public List<PathPart> GetPathParts()
        {
            return _pathParts;
        }

        public float GetWidth()
        {
            return width;
        }

        public float GetHeight()
        {
            return height;
        }
    }

    public sealed class PathPart
    {
        public PathPart(PathPartType type, List<Coordinate> coordinates)
        {
            Coordinates = coordinates;
            Type = type;
        }

        public List<Coordinate> Coordinates { get; }
        public PathPartType Type { get; }
    }

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
        Z = 9,
    }
}