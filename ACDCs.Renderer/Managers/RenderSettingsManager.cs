using ACDCs.Interfaces;
using ACDCs.Interfaces.Drawing;
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
    /// <param name="drawing">The drawing.</param>
    public static void ApplyColors(ICanvas canvas, IDrawing? drawing = null)
    {
        canvas.StrokeSize = drawing?.StrokeSize ?? 2;
        canvas.StrokeColor = drawing?.LineColor ?? GetStrokeColor(drawing);
        canvas.FontColor = drawing?.LineColor ?? GetFontColor(drawing);
        canvas.FillColor = drawing?.BackgroundColor ?? GetFillColor(drawing);
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
    /// <param name="drawing">The drawing.</param>
    /// <returns></returns>
    public static Color GetFillColor(IDrawing? drawing)
    {
        return drawing?.ParentDrawing?.BackgroundColor ?? GetColor(ColorDefinition.CircuitRendererFill);
    }

    /// <summary>
    /// Gets the color of the font.
    /// </summary>
    /// <param name="drawing">The drawing.</param>
    /// <returns></returns>
    public static Color GetFontColor(IDrawing? drawing)
    {
        return drawing?.ParentDrawing?.LineColor ?? GetColor(ColorDefinition.CircuitRendererFont);
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
    /// <param name="drawing">The drawing.</param>
    /// <returns></returns>
    public static Color GetStrokeColor(IDrawing? drawing)
    {
        return drawing?.ParentDrawing?.LineColor ?? GetColor(ColorDefinition.CircuitRendererStroke);
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