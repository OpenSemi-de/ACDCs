namespace ACDCs.CircuitRenderer.Sheet;

public static class DirectionNineExtensions
{
    public static DirectionNine GetOpposite(this DirectionNine direction)
    {
        if (GetNum(direction) > 5)
        {
            return ByNum(direction, -4);
        }
        else
        {
            return ByNum(direction, 4);
        }
    }

    public static DirectionNine Turn(this DirectionNine direction)
    {
        if (GetNum(direction) > 7)
        {
            return ByNum(direction, -6);
        }
        else
        {
            return ByNum(direction, 2);
        }
    }

    private static DirectionNine ByNum(this DirectionNine direction, int i)
    {
        return (DirectionNine)((int)direction + i);
    }

    private static int GetNum(this DirectionNine direction)
    {
        return (int)direction;
    }
}
