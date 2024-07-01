using ACDCs.Interfaces;
using ACDCs.Structs;

namespace ACDCs.Services;

/// <summary>
/// The color service implementation.
/// </summary>
/// <seealso cref="ACDCs.Interfaces.IThemeService" />
public class ThemeService : IThemeService
{
    private readonly Dictionary<ColorDefinition, Color> _colorsDark;
    private readonly Dictionary<ColorDefinition, Color> _colorsLight;
    private readonly AppTheme _theme;

    /// <summary>
    /// Initializes a new instance of the <see cref="ThemeService"/> class.
    /// </summary>
    /// <param name="theme">The theme.</param>
    public ThemeService(AppTheme? theme)
    {
        _colorsLight = new()
        {
            { ColorDefinition.Background, Colors.White },
            { ColorDefinition.Foreground, Colors.Black },
            { ColorDefinition.Border, Colors.DeepSkyBlue },
            { ColorDefinition.StartButtonPressed, Colors.LightSkyBlue },
            { ColorDefinition.StartButtonReleased, Colors.White },
            { ColorDefinition.StartMenuBackground, Colors.LightSkyBlue.WithAlpha(0.7f) },
            { ColorDefinition.ModuleBackground, Colors.LightSkyBlue },
            { ColorDefinition.ModuleTitleBackground, Colors.White.WithAlpha(0.5f) },
            { ColorDefinition.Shadow, Colors.Black },
            { ColorDefinition.CircuitBackground, Colors.LightBlue },
            { ColorDefinition.CircuitControllerBackground, Colors.LightBlue.AddLuminosity(-0.2f) },
            { ColorDefinition.CircuitComponentBackground, Colors.LightBlue.AddLuminosity(-0.2f) },
            { ColorDefinition.CircuitRendererBackground, Colors.LightBlue.AddLuminosity(-0.2f) },
            { ColorDefinition.CircuitRendererStroke, Colors.Black.AddLuminosity(0.1f) },
            { ColorDefinition.CircuitRendererFill, Colors.LightBlue.AddLuminosity(0.2f) },
            { ColorDefinition.CircuitRendererFont, Colors.Black.AddLuminosity(0.1f) },
            { ColorDefinition.CircuitRendererGrid, Colors.DarkBlue.AddLuminosity(0.4f) },
        };

        _colorsDark = new()
        {
            { ColorDefinition.Background, Colors.Black },
            { ColorDefinition.Foreground, Colors.White },
            { ColorDefinition.Border, Colors.LightSlateGray },
            { ColorDefinition.StartButtonPressed, Colors.DarkSlateBlue },
            { ColorDefinition.StartButtonReleased, Colors.Black },
            { ColorDefinition.StartMenuBackground, Colors.DarkSlateBlue.WithAlpha(0.7f) },
            { ColorDefinition.ModuleBackground, Colors.DarkSlateBlue },
            { ColorDefinition.ModuleTitleBackground, Colors.Black.WithAlpha(0.5f) },
            { ColorDefinition.Shadow, Colors.White },
            { ColorDefinition.CircuitBackground, Colors.DarkBlue },
            { ColorDefinition.CircuitControllerBackground, Colors.DarkBlue.AddLuminosity(0.2f) },
            { ColorDefinition.CircuitComponentBackground, Colors.DarkBlue.AddLuminosity(0.2f) },
            { ColorDefinition.CircuitRendererBackground, Colors.DarkBlue.AddLuminosity(0.1f) },
            { ColorDefinition.CircuitRendererStroke, Colors.White.AddLuminosity(-0.1f) },
            { ColorDefinition.CircuitRendererFill, Colors.DarkBlue.AddLuminosity(0.2f) },
            { ColorDefinition.CircuitRendererFont, Colors.White.AddLuminosity(-0.1f) },
            { ColorDefinition.CircuitRendererGrid, Colors.LightBlue.AddLuminosity(-0.4f) },
        };

        _theme = theme ?? AppTheme.Unspecified;
    }

    /// <summary>
    /// Occurs when [theme changed].
    /// </summary>
    public event EventHandler? ThemeChanged;

    /// <summary>
    /// Gets the color.
    /// </summary>
    /// <param name="colorDefinition">The color definition.</param>
    /// <returns></returns>
    public Color GetColor(ColorDefinition colorDefinition)
    {
        return _theme != AppTheme.Dark ?
                        _colorsLight[colorDefinition] :
                        _colorsDark[colorDefinition];
    }

    /// <summary>
    /// Sets the theme.
    /// </summary>
    /// <param name="requestedTheme">The requested theme.</param>
    /// <exception cref="NotImplementedException"></exception>
    public void SetTheme(AppTheme requestedTheme)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Raises the <see cref="E:ThemeChanged" /> event.
    /// </summary>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    protected virtual void OnThemeChanged(EventArgs e)
    {
        ThemeChanged?.Invoke(this, e);
    }
}