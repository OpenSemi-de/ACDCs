using ACDCs.Structs;

namespace ACDCs.Shared;

/// <summary>
/// Static helper class for selection calculation.
/// </summary>
public static class SelectionHelper
{
    /// <summary>
    /// Gets if the points is in the rect.
    /// </summary>
    /// <param name="point">The p1.</param>
    /// <param name="rect">The r.</param>
    /// <returns></returns>
    public static bool PointInRect(Point point, Quad rect)
    {
        if (PointInTriangle(point, new Point(rect.X1, rect.Y1),
                new Point(rect.X2, rect.Y2), new Point(rect.X4, rect.Y4)))
            return true;
        if (PointInTriangle(point, new Point(rect.X1, rect.Y1),
                new Point(rect.X2, rect.Y2), new Point(rect.X3, rect.Y3)))
            return true;

        return false;
    }

    /// <summary>
    /// Checks wether a points is in a triangle.
    /// </summary>
    /// <param name="pt">The pt.</param>
    /// <param name="v1">The v1.</param>
    /// <param name="v2">The v2.</param>
    /// <param name="v3">The v3.</param>
    /// <returns></returns>
    public static bool PointInTriangle(
                Point pt,
                Point v1,
                Point v2,
                Point v3)
    {
        float d1 = Sign(pt, v1, v2);
        float d2 = Sign(pt, v2, v3);
        float d3 = Sign(pt, v3, v1);

        bool hasNeg = d1 < 0 || d2 < 0 || d3 < 0;
        bool hasPos = d1 > 0 || d2 > 0 || d3 > 0;

        return !(hasNeg && hasPos);
    }

    private static float Sign(
            Point p1,
            Point p2,
            Point p3)
    {
        return Convert.ToSingle((p1.X - p3.X) * (p2.Y - p3.Y) - (p2.X - p3.X) * (p1.Y - p3.Y));
    }
}