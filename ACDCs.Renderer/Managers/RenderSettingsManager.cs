using ACDCs.Interfaces;
using System.Collections.Concurrent;

namespace ACDCs.Renderer.Managers;

/// <summary>
/// Manager to handle fast access to render colors.
/// </summary>
public static class RenderSettingsManager
{
    private static ConcurrentDictionary<ColorDefinition, Color> s_colorCache = new();
    private static IThemeService? s_themeService;

    /// <summary>
    /// Applies the colors.
    /// </summary>
    /// <param name="canvas">The canvas.</param>
    public static void ApplyColors(ICanvas canvas)
    {
        canvas.StrokeColor = GetStrokeColor();
        canvas.FontColor = GetFontColor();
        canvas.FillColor = GetFillColor();
    }

    /// <summary>
    /// Gets the color.
    /// </summary>
    /// <param name="definition">The definition.</param>
    /// <returns></returns>
    public static Color GetColor(ColorDefinition definition)
    {
        Color? color;

        if (s_colorCache.TryGetValue(definition, out Color? cachedColor))
        {
            color = cachedColor;
        }
        else
        {
            color = s_themeService?.GetColor(definition);
            if (color != null)
            {
                s_colorCache.TryAdd(definition, color);
            }
        }

        if (color == null)
        {
            return Colors.Black;
        }

        return color;
    }

    /// <summary>
    /// Gets the color of the fill.
    /// </summary>
    /// <returns></returns>
    public static Color GetFillColor()
    {
        return GetColor(ColorDefinition.CircuitRendererFill);
    }

    /// <summary>
    /// Gets the color of the font.
    /// </summary>
    /// <returns></returns>
    public static Color GetFontColor()
    {
        return GetColor(ColorDefinition.CircuitRendererFont);
    }

    /// <summary>
    /// Gets the color of the grid.
    /// </summary>
    /// <returns></returns>
    public static Color GetGridColor()
    {
        return GetColor(ColorDefinition.CircuitRendererGrid);
    }

    /// <summary>
    /// Gets the color of the stroke.
    /// </summary>
    /// <returns></returns>
    public static Color GetStrokeColor()
    {
        return GetColor(ColorDefinition.CircuitRendererStroke);
    }

    /// <summary>
    /// Sets the service.
    /// </summary>
    /// <param name="service">The service.</param>
    public static void SetService(IThemeService service)
    {
        s_themeService = service;
    }
}