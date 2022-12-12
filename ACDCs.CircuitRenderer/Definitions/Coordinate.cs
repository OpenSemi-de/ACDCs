using System;
using Microsoft.Maui.Graphics;

namespace ACDCs.CircuitRenderer.Definitions;

public sealed class Coordinate
{
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

    public float X;

    public float Y;

    public float Z;

    public static Coordinate FromPoint(Point stepPoint)
    {
        return new Coordinate(Convert.ToSingle(stepPoint.X), Convert.ToSingle(stepPoint.Y), 0);
    }

    public Coordinate Add(Coordinate? coordinate)
    {
        if (coordinate != null)
        {
            return new Coordinate(
                X + coordinate.X,
                Y + coordinate.Y,
                Z + coordinate.Z
                                 );
        }

        return this;
    }

    public Coordinate Round()
    {
        X = (float)Math.Round(X);
        Y = (float)Math.Round(Y);
        Z = (float)Math.Round(Z);
        return this;
    }

    public Coordinate Substract(Coordinate? coordinate)
    {
        if (coordinate != null)
        {
            return new Coordinate(
                X - coordinate.X,
                Y - coordinate.Y,
                Z - coordinate.Z
                                 );
        }

        return this;
    }

    public PointF ToPointF()
    {
        return new PointF(X, Y);
    }

    public SizeF ToSizeF()
    {
        return new SizeF(X, Y);
    }
}
