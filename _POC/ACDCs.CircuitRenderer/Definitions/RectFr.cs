#nullable enable

namespace ACDCs.CircuitRenderer.Definitions;

public class RectFr
{
    public float X1 { get; set; }

    public float X2 { get; set; }

    public float X3 { get; set; }

    public float X4 { get; set; }

    public float Y1 { get; set; }

    public float Y2 { get; set; }

    public float Y3 { get; set; }

    public float Y4 { get; set; }

    public RectFr(int x1, int y1, int x2, int y2, int x3, int y3, int x4, int y4)
    {
        X1 = x1;
        Y1 = y1;
        X2 = x2;
        Y2 = y2;
        X3 = x3;
        Y3 = y3;
        X4 = x4;
        Y4 = y4;
    }
}
