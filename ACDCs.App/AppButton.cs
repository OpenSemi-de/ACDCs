using ACDCs.Interfaces;

namespace ACDCs.App;

using ACDCs.Structs;
using Sharp.UI;

/// <summary>
/// The Button Class with app extensions.
/// </summary>
/// <seealso cref="Sharp.UI.Button" />
public class AppButton : Border
{
    private readonly Button _button;
    private readonly IThemeService _themeService;

    /// <summary>
    /// Initializes a new instance of the <see cref="AppButton"/> class.
    /// </summary>
    /// <param name="themeService">The color service.</param>
    public AppButton(IThemeService themeService)
    {
        _themeService = themeService;
        _button = new();

        _button.TextColor(_themeService.GetColor(ColorDefinition.Foreground))
            .BackgroundColor(_themeService.GetColor(ColorDefinition.Background))
            .BorderColor(_themeService.GetColor(ColorDefinition.Border));

        _button.Clicked += AppButton_Clicked;

        Content = _button;

        _themeService.ThemeChanged += ThemeService_ThemeChanged;
    }

    /// <summary>
    /// Occurs when [clicked].
    /// </summary>
    public event EventHandler? Clicked;

    /// <summary>
    /// Gets or sets the command parameter.
    /// </summary>
    /// <value>
    /// The command parameter.
    /// </value>
    public object? CommandParameter { get; set; }

    /// <summary>
    /// Sets the specified text.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <returns></returns>
    public AppButton Text(string text)
    {
        _button.Text(text);
        return this;
    }

    private void AppButton_Clicked(object? sender, EventArgs e)
    {
        _button.CancelAnimations();
        _button.BackgroundColor(_themeService.GetColor(ColorDefinition.Foreground));
        Clicked?.Invoke(this, e);
        _button.AnimateBackgroundColorTo(_themeService.GetColor(ColorDefinition.Background), 500, Easing.Linear);
    }

    private void ThemeService_ThemeChanged(object? sender, EventArgs e)
    {
        _button.TextColor(_themeService.GetColor(ColorDefinition.Foreground))
            .BackgroundColor(_themeService.GetColor(ColorDefinition.Background))
            .BorderColor(_themeService.GetColor(ColorDefinition.Border));
    }
}