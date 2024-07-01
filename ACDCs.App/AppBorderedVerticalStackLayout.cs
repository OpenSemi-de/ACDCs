using ACDCs.Interfaces;

namespace ACDCs.App;

using ACDCs.Structs;
using Sharp.UI;

/// <summary>
/// App-specific component: A bordered VerticalStackLayout.
/// </summary>
/// <seealso cref="Sharp.UI.Border" />
public class AppBorderedVerticalStackLayout : Border
{
    private readonly SolidColorBrush _solidColorBrush;
    private readonly IThemeService _themeService;
    private readonly VerticalStackLayout layout = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="AppBorderedVerticalStackLayout" /> class.
    /// </summary>
    /// <param name="themeService">The theme service.</param>
    public AppBorderedVerticalStackLayout(IThemeService themeService)
    {
        _themeService = themeService;
        _themeService.ThemeChanged += ThemeService_ThemeChanged;

        _solidColorBrush = new SolidColorBrush(_themeService.GetColor(ColorDefinition.Border));
        Stroke = _solidColorBrush;
        StrokeThickness = 2;

        Content = layout;
        layout.HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill);
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