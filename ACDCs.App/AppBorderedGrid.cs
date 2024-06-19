namespace ACDCs.App.Desktop;

using ACDCs.Interfaces;
using Sharp.UI;
using System.Collections.Generic;

/// <summary>
/// App specific component: A bordered GridView.
/// </summary>
/// <seealso cref="Sharp.UI.Border" />
public class AppBorderedGrid : Border
{
    private readonly Grid _grid = [];
    private readonly SolidColorBrush _stroke = new();
    private readonly IThemeService _themeService;

    /// <summary>
    /// Initializes a new instance of the <see cref="AppBorderedGrid" /> class.
    /// </summary>
    /// <param name="themeService">The theme service.</param>
    public AppBorderedGrid(IThemeService themeService)
    {
        Content = _grid;
        _themeService = themeService;
        _stroke.Color = _themeService.GetColor(ColorDefinition.Border);

        StrokeThickness = 2;
        Stroke = _stroke;

        Shadow = new()
        {
            Offset = new Microsoft.Maui.Graphics.Point(5, 5),
            Brush = new SolidColorBrush(_themeService.GetColor(ColorDefinition.Shadow)),
            Opacity = 0.50f
        };
        _themeService.ThemeChanged += ThemeService_ThemeChanged;
    }

    /// <summary>
    /// Gets the children.
    /// </summary>
    /// <value>
    /// The children.
    /// </value>
    public IList<IView> Children => _grid.Children;

    /// <summary>
    /// Rows the definitions.
    /// </summary>
    /// <param name="rowDefinitions">The row definitions.</param>
    public void RowDefinitions(RowDefinition[] rowDefinitions)
    {
        _grid.RowDefinitions(rowDefinitions);
    }

    private void ThemeService_ThemeChanged(object? sender, EventArgs e)
    {
        _stroke.Color = _themeService.GetColor(ColorDefinition.Border);
        Shadow.Brush = new SolidColorBrush(_themeService.GetColor(ColorDefinition.Shadow));
    }
}