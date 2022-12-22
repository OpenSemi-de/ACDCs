using System;
using Microsoft.Maui.Graphics;

namespace ACDCs.CircuitRenderer.Definitions;

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

    public bool IsEqual(Coordinate coordinate)
    {
        return Math.Abs(X - coordinate.X) < 0.01 &&
               Math.Abs(Y - coordinate.Y) < 0.01 &&
               Math.Abs(Z - coordinate.Z) < 0.01;
    }

    public bool IsNaN()
    {
        return float.IsNaN(X) && float.IsNaN(Y) && float.IsNaN(Z);
    }

    public Coordinate Multiply(Coordinate coordinate)
    {
        var result = new Coordinate(this);
        result.X *= coordinate.X;
        result.Y *= coordinate.Y;
        result.Z *= coordinate.Z;
        return result;
    }

    public Coordinate Multiply(float multplier)
    {
        return new Coordinate(
            X * multplier,
            Y * multplier,
            Z * multplier
        );
    }

    public Coordinate RotateCoordinate(float centerX, float centerY, double angleInDegrees)
    {
        double angleInRadians = angleInDegrees * (Math.PI / 180);
        double cosTheta = Math.Cos(angleInRadians);
        double sinTheta = Math.Sin(angleInRadians);
        return new Coordinate
        {
            X =
                Convert.ToSingle(
                    (cosTheta * (X - centerX) -
                        sinTheta * (Y - centerY) + centerX)),
            Y =
                Convert.ToSingle(
                    (sinTheta * (X - centerX) +
                     cosTheta * (Y - centerY) + centerY))
        };
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

    public override string ToString()
    {
        return $"{X}/{Y}/{Z}-Coordinate";
    }
}
