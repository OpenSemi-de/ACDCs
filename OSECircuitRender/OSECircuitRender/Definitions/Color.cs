using System;

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

    public Color(Microsoft.Maui.Graphics.Color color)
    {
        this.A = Convert.ToInt32(color.Alpha * 255);
        this.R = Convert.ToInt32(color.Red * 255);
        this.G = Convert.ToInt32(color.Green * 255);
        this.B = Convert.ToInt32(color.Blue * 255);
    }

    public Microsoft.Maui.Graphics.Color ToMauiColor()
    {
        return new Microsoft.Maui.Graphics.Color(
            R / 255f,
            G / 255f,
            B / 255f
        );
    }
}
