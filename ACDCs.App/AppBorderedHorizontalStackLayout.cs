using ACDCs.Interfaces;

namespace ACDCs.App;

using ACDCs.Structs;
using Sharp.UI;

/// <summary>
/// App-specific component: A bordered HorizontalStackLayout.
/// </summary>
/// <seealso cref="Sharp.UI.Border" />
public class AppBorderedHorizontalStackLayout : Border
{
    private readonly SolidColorBrush _solidColorBrush;
    private readonly IThemeService _themeService;
    private readonly HorizontalStackLayout layout = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="AppBorderedHorizontalStackLayout" /> class.
    /// </summary>
    /// <param name="themeService">The theme service.</param>
    public AppBorderedHorizontalStackLayout(IThemeService themeService)
    {
        _themeService = themeService;

        Content = layout;
        layout.HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill);

        _solidColorBrush = new SolidColorBrush(_themeService.GetColor(ColorDefinition.Border));
        Stroke = _solidColorBrush;
        StrokeThickness = 2;

        themeService.ThemeChanged += ThemeService_ThemeChanged;
    }

    /// <summary>
    /// Gets the children.
    /// </summary>
    /// <value>
    /// The children.
    /// </value>
    public IList<IView> Children => layout.Children;

    private void ThemeService_ThemeChanged(object? sender, EventArgs e)
    {
        _solidColorBrush.Color = _themeService.GetColor(ColorDefinition.Border);
    }
}