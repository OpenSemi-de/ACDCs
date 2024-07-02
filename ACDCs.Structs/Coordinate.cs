using System;
using Microsoft.Maui.Graphics;

namespace ACDCs.Structs;

/// <summary>
/// Coordinate class to handle three dimensional points.
/// </summary>
public sealed class Coordinate
{
    /// <summary>
    /// The x
    /// </summary>
    public float X;

    /// <summary>
    /// The y
    /// </summary>
    public float Y;

    /// <summary>
    /// The z
    /// </summary>
    public float Z;

    /// <summary>
    /// Initializes a new instance of the <see cref="Coordinate"/> class.
    /// </summary>
    /// <param name="x">The x.</param>
    /// <param name="y">The y.</param>
    /// <param name="z">The z.</param>
    public Coordinate(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Coordinate"/> class.
    /// </summary>
    /// <param name="x">The x.</param>
    /// <param name="y">The y.</param>
    public Coordinate(float x, float y)
    {
        X = x;
        Y = y;
        Z = 0;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Coordinate"/> class.
    /// </summary>
    /// <param name="coordinate">The coordinate.</param>
    public Coordinate(Coordinate coordinate)
    {
        X = coordinate.X;
        Y = coordinate.Y;
        Z = coordinate.Z;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Coordinate"/> class.
    /// </summary>
    public Coordinate()
    {
    }

    /// <summary>
    /// Froms the point.
    /// </summary>
    /// <param name="stepPoint">The step point.</param>
    /// <returns></returns>
    public static Coordinate FromPoint(Point stepPoint)
    {
        return new Coordinate(Convert.ToSingle(stepPoint.X), Convert.ToSingle(stepPoint.Y), 0);
    }

    /// <summary>
    /// Adds the specified coordinate.
    /// </summary>
    /// <param name="coordinate">The coordinate.</param>
    /// <returns></returns>
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

    /// <summary>
    /// Determines whether the specified coordinate is equal.
    /// </summary>
    /// <param name="coordinate">The coordinate.</param>
    /// <returns>
    ///   <c>true</c> if the specified coordinate is equal; otherwise, <c>false</c>.
    /// </returns>
    public bool IsEqual(Coordinate coordinate)
    {
        return Math.Abs(X - coordinate.X) < 0.01 &&
               Math.Abs(Y - coordinate.Y) < 0.01 &&
               Math.Abs(Z - coordinate.Z) < 0.01;
    }

    /// <summary>
    /// Determines whether [is na n].
    /// </summary>
    /// <returns>
    ///   <c>true</c> if [is na n]; otherwise, <c>false</c>.
    /// </returns>
    public bool IsNaN()
    {
        return float.IsNaN(X) && float.IsNaN(Y) && float.IsNaN(Z);
    }

    /// <summary>
    /// Multiplies the specified coordinate.
    /// </summary>
    /// <param name="coordinate">The coordinate.</param>
    /// <returns></returns>
    public Coordinate Multiply(Coordinate coordinate)
    {
        var result = new Coordinate(this);
        result.X *= coordinate.X;
        result.Y *= coordinate.Y;
        result.Z *= coordinate.Z;
        return result;
    }

    /// <summary>
    /// Multiplies the specified multplier.
    /// </summary>
    /// <param name="multplier">The multplier.</param>
    /// <returns></returns>
    public Coordinate Multiply(float multplier)
    {
        return new Coordinate(
            X * multplier,
            Y * multplier,
            Z * multplier
        );
    }

    /// <summary>
    /// Rotates the coordinate.
    /// </summary>
    /// <param name="centerX">The center x.</param>
    /// <param name="centerY">The center y.</param>
    /// <param name="angleInDegrees">The angle in degrees.</param>
    /// <returns></returns>
    public Coordinate RotateCoordinate(float centerX, float centerY, double angleInDegrees)
    {
        double angleInRadians = angleInDegrees * (Math.PI / 180);
        double cosTheta = Math.Cos(angleInRadians);
        double sinTheta = Math.Sin(angleInRadians);
        return new Coordinate
        {
            X =
                Convert.ToSingle(
                    cosTheta * (X - centerX) -
                    sinTheta * (Y - centerY) + centerX),
            Y =
                Convert.ToSingle(
                    sinTheta * (X - centerX) +
                    cosTheta * (Y - centerY) + centerY)
        };
    }

    /// <summary>
    /// Rounds this instance.
    /// </summary>
    /// <returns></returns>
    public Coordinate Round()
    {
        X = (float)Math.Round(X);
        Y = (float)Math.Round(Y);
        Z = (float)Math.Round(Z);
        return this;
    }

    /// <summary>
    /// Substracts the specified coordinate.
    /// </summary>
    /// <param name="coordinate">The coordinate.</param>
    /// <returns></returns>
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

    /// <summary>
    /// Converts to pointf.
    /// </summary>
    /// <returns></returns>
    public PointF ToPointF()
    {
        return new PointF(X, Y);
    }

    /// <summary>
    /// Converts to sizef.
    /// </summary>
    /// <returns></returns>
    public SizeF ToSizeF()
    {
        return new SizeF(X, Y);
    }

    /// <summary>
    /// Converts to string.
    /// </summary>
    /// <returns>
    /// A <see cref="System.String" /> that represents this instance.
    /// </returns>
    public override string ToString()
    {
        return $"{X}/{Y}/{Z}-Coordinate";
    }
}