using ACDCs.Interfaces;

namespace ACDCs.App;

using MauiIcons.Core;
using Sharp.UI;

/// <summary>
/// The Button Class with app extensions.
/// </summary>
/// <seealso cref="Sharp.UI.Button" />
public class AppImageButton : Border
{
    private readonly ImageButton _button;
    private readonly IThemeService _themeService;
    private ImageSource? _imageSource;

    /// <summary>
    /// Initializes a new instance of the <see cref="AppButton"/> class.
    /// </summary>
    /// <param name="themeService">The color service.</param>
    public AppImageButton(IThemeService themeService)
    {
        _themeService = themeService;
        _button = new();

        _button.BackgroundColor(_themeService.GetColor(ColorDefinition.Background))
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
    /// <param name="icon">The icon.</param>
    /// <returns></returns>
    public AppImageButton Icon(Enum icon)
    {
        _imageSource = icon.ToImageSource();
        _button.Source = _imageSource;
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
        _button.BackgroundColor(_themeService.GetColor(ColorDefinition.Background))
            .BorderColor(_themeService.GetColor(ColorDefinition.Border));
    }
}