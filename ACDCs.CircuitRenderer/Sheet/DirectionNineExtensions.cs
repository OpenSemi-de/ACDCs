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

    public static void Turn(this ref DirectionNine direction, int i)
    {
        if (i > 0)
        {
            if (GetNum(direction) > 7)
            {
                direction = ByNum(direction, i - 8);
            }
            else
            {
                direction = ByNum(direction, i);
            }
        }
        else
        {
            if (GetNum(direction) < 2)
            {
                direction = ByNum(direction, i + 8);
            }
            else
            {
                direction = ByNum(direction, i);
            }
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
