using System;
using Microsoft.Maui.Graphics;

namespace OSECircuitRender.Definitions;

public sealed class Coordinate
{
    public float X;

    public float Y;

    public float Z;

    public Coordinate(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public Coordinate(float x, float y)
    {
        X = x;
        Y = y;
        Z = 0;
    }

    public Coordinate(Coordinate coordinate)
    {
        X = coordinate.X;
        Y = coordinate.Y;
        Z = coordinate.Z;
    }

    public Coordinate()
    {
    }

    public static Coordinate FromPoint(Point stepPoint)
    {
        return new Coordinate(Convert.ToSingle(stepPoint.X), Convert.ToSingle(stepPoint.Y), 0);
    }

    public Coordinate Add(Coordinate coordinate)
    {
        return new Coordinate(
            X + coordinate.X,
            Y + coordinate.Y,
            Z + coordinate.Z
        );
    }

    public Coordinate Substract(Coordinate coordinate)
    {
        return new Coordinate(
            X - coordinate.X,
            Y - coordinate.Y,
            Z - coordinate.Z
        );
    }
}