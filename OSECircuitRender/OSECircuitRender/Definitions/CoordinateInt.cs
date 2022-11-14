namespace OSECircuitRender.Definitions;

public sealed class CoordinateInt
{
    public int X;

    public int Y;

    public int Z;

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

    public CoordinateInt Add(CoordinateInt coordinate)
    {
        return new CoordinateInt(
            X + coordinate.X,
            Y + coordinate.Y,
            Z + coordinate.Z
        );
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