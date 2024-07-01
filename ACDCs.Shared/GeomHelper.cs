namespace ACDCs.Shared;

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
    public static Rect FromRect(this Rect input)
    {
        Rect rect = new(input.X, input.Y, input.Width, input.Height);
        return rect;
    }

    /// <summary>
    /// Converts to rect.
    /// </summary>
    /// <param name="input">The input.</param>
    /// <returns></returns>
    public static Rect ToRect(this Rect input)
    {
        Rect rect = new(input.X, input.Y, input.Width, input.Height);
        return rect;
    }
}