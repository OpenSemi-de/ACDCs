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

    public Coordinate(Coordinate coordinate)
    {
        X = coordinate.X;
        Y = coordinate.Y;
        Z = coordinate.Z;
    }

    public Coordinate()
    {
    }

    public Coordinate Add(Coordinate drawableComponentPosition)
    {
        return new Coordinate(
            X + drawableComponentPosition.X,
            Y + drawableComponentPosition.Y,
            Z + drawableComponentPosition.Z
        );
    }

    public Coordinate Substract(Coordinate drawableComponentPosition)
    {
        return new Coordinate(
            X - drawableComponentPosition.X,
            Y - drawableComponentPosition.Y,
            Z - drawableComponentPosition.Z
        );
    }
}