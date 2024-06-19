using System;

namespace ACDCs.CircuitRenderer.Definitions;

public sealed class Color
{
    public int A;

    public int B;

    public int G;

    public int R;

    public Color()
    {
        R = 255;
        G = 255;
        B = 255;
        A = 255;
    }

    public Color(int r, int g, int b, int a = 255)
    {
        R = r;
        G = g;
        B = b;
        A = a;
    }

    public Color(Microsoft.Maui.Graphics.Color color)
    {
        A = Convert.ToInt32(color.Alpha * 255);
        R = Convert.ToInt32(color.Red * 255);
        G = Convert.ToInt32(color.Green * 255);
        B = Convert.ToInt32(color.Blue * 255);
    }

    public Microsoft.Maui.Graphics.Color ToMauiColor()
    {
        return new Microsoft.Maui.Graphics.Color(
            R / 255f,
            G / 255f,
            B / 255f,
            A / 255f
        );
    }
}
