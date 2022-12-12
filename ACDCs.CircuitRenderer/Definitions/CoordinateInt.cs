using System;

namespace ACDCs.CircuitRenderer.Definitions;

public sealed class CoordinateInt
{
    public CoordinateInt(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public CoordinateInt(CoordinateInt coordinate)
    {
        X = coordinate.X;
        Y = coordinate.Y;
        Z = coordinate.Z;
    }

    public CoordinateInt()
    {
    }

    public int X;

    public int Y;

    public int Z;

    public static CoordinateInt FromFloat(Coordinate coordinate)
    {
        var newCoordinate = new CoordinateInt(
            Convert.ToInt32(Math.Round(coordinate.X)),
            Convert.ToInt32(Math.Round(coordinate.Y)),
            Convert.ToInt32(Math.Round(coordinate.Z))
        );

        return newCoordinate;
    }

    public CoordinateInt Add(CoordinateInt coordinate)
    {
        return new CoordinateInt(
            X + coordinate.X,
            Y + coordinate.Y,
            Z + coordinate.Z
        );
    }

    public bool IsEqual(CoordinateInt end)
    {
        return X == end.X && Y == end.Y && Z == end.Z;
    }

    public CoordinateInt Substract(CoordinateInt coordinate)
    {
        return new CoordinateInt(
            X - coordinate.X,
            Y - coordinate.Y,
            Z - coordinate.Z
        );
    }
}
