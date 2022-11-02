using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;

namespace OSECircuitRender
{
    public sealed class Path : DrawInstruction
    {
        private List<PathPart> pathParts;

        public Path(string svgPath) : base(typeof(Path))
        {
            Colors.Add(new Color(0, 0, 0));
            PathReader pr = new(svgPath);
            pathParts = pr.GetPathParts();
        }
    }

    public sealed class PathReader
    {
        private readonly List<PathPart> pathParts = new();

        public PathReader(string svgPath)
        {
            svgPath += "Z";
            string buffer = "";
            string command = "";
            for (var i = 0; i < svgPath.Length; i++)
            {
                char chrBuffer = svgPath[i];
                if ((chrBuffer > 64 && chrBuffer < 91) ||
                    (chrBuffer > 96 && chrBuffer < 123))
                {
                    if (command != "")
                    {
                        List<DrawCoordinate> coordinates = new();
                        PathPartType type = (PathPartType)Enum.Parse(typeof(PathPartType), command);
                        List<string> textCoordinates = buffer.Split(" ").ToList();
                        foreach (var textCoordinate in textCoordinates.Where(s => s != "" && s != " "))
                        {
                            var xypair = textCoordinate.Split(',');
                            coordinates.Add(new DrawCoordinate(
                                float.Parse(xypair[0].Replace(".", ",")),
                                float.Parse(xypair[1].Replace(".", ",")),
                                0
                                ));
                        }

                        pathParts.Add(new PathPart(type, coordinates));
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
            return pathParts;
        }
    }

    public sealed class PathPart
    {
        public PathPart(PathPartType type, List<DrawCoordinate> coordinates)
        {
            Coordinates = coordinates;
            Type = type;
        }

        public List<DrawCoordinate> Coordinates { get; }
        public PathPartType Type { get; }
    }

    public enum PathPartType
    {
        M = 0,// moveto,
        L = 1,//lineto
        H = 2,//horizontal lineto
        V = 3,//vertical lineto
        C = 4,//curveto
        S = 5,//smooth curveto
        Q = 6,//quadratic Bézier curve
        T = 7,//smooth quadratic Bézier curveto
        A = 8,//elliptical Arc
        Z = 9,//closepath
    }
}