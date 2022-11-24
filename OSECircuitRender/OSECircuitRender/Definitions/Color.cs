namespace OSECircuitRender.Definitions;

public sealed class Color
{
    public int A;
    public int B;
    public int G;

    public int R;

    public Color(int r, int g, int b, int a = 100)
    {
        R = r;
        G = g;
        B = b;
        A = a;
    }
}