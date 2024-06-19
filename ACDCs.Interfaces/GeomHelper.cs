namespace ACDCs.Interfaces;

/// <summary>
/// Helper class from maui.graphics geometry to app internal.
/// </summary>
public static class GeomHelper
{
    /// <summary>
    /// Froms the rect.
    /// </summary>
    /// <param name="input">The input.</param>
    /// <returns></returns>
    public static Microsoft.Maui.Graphics.Rect FromRect(this Rect input)
    {
        Microsoft.Maui.Graphics.Rect rect = new(input.X, input.Y, input.Width, input.Height);
        return rect;
    }

    /// <summary>
    /// Converts to rect.
    /// </summary>
    /// <param name="input">The input.</param>
    /// <returns></returns>
    public static Rect ToRect(this Microsoft.Maui.Graphics.Rect input)
    {
        Rect rect = new(input.X, input.Y, input.Width, input.Height);
        return rect;
    }
}