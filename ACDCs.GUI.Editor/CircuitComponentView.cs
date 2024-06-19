namespace ACDCs.App.GUI.Modules;

using ACDCs.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Layouts;
using Sharp.UI;

/// <summary>
/// The view for the circuit components.
/// </summary>
/// <seealso cref="ACDCs.Interfaces.ICircuitComponentView" />
/// <seealso cref="Sharp.UI.ContentView" />
public class CircuitComponentView : ContentView, ICircuitComponentView
{
    private readonly ILogger _logger;
    private readonly IThemeService _themeService;

    /// <summary>
    /// Initializes a new instance of the <see cref="CircuitEditorView" /> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="themeService">The theme service.</param>
    public CircuitComponentView(ILogger logger, IThemeService themeService)
    {
        _logger = logger;
        _themeService = themeService;
        this.AbsoluteLayoutBounds(0, 1, 1, 40)
            .AbsoluteLayoutFlags(AbsoluteLayoutFlags.YProportional | AbsoluteLayoutFlags.WidthProportional)
            .BackgroundColor = _themeService.GetColor(ColorDefinition.CircuitComponentBackground);

        _logger.LogDebug("Circuit components started.");
    }

    /// <summary>
    /// Gets or sets the render core.
    /// </summary>
    /// <value>
    /// The render core.
    /// </value>
    public IRenderManager? RenderCore { get; set; }
}