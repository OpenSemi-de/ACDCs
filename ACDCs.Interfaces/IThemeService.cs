namespace ACDCs.Interfaces;

/// <summary>
/// Interface for the theme service
/// </summary>
public interface IThemeService
{
    /// <summary>
    /// Occurs when [theme changed].
    /// </summary>
    event EventHandler ThemeChanged;

    /// <summary>
    /// Gets the color.
    /// </summary>
    /// <param name="colorDefinition">The color definition.</param>
    /// <returns></returns>
    Color GetColor(ColorDefinition colorDefinition);

    /// <summary>
    /// Sets the theme.
    /// </summary>
    /// <param name="requestedTheme">The requested theme.</param>
    void SetTheme(AppTheme requestedTheme);
}