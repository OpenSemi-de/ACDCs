namespace ACDCs.App.GUI.Modules;

using ACDCs.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Layouts;
using Sharp.UI;

/// <summary>
/// The view for the circuit controller to start/stop/pause the circuit.
/// </summary>
/// <seealso cref="Sharp.UI.ContentView" />
/// <seealso cref="ACDCs.Interfaces.ICircuitControllerView" />
public class CircuitControllerView : ContentView, ICircuitControllerView
{
    private readonly ILogger _logger;
    private readonly IThemeService _themeService;

    /// <summary>
    /// Initializes a new instance of the <see cref="CircuitControllerView"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="themeService">The theme service.</param>
    public CircuitControllerView(ILogger logger, IThemeService themeService)
    {
        _logger = logger;
        _themeService = themeService;
        this.AbsoluteLayoutBounds(0, 0, 1, 32)
            .AbsoluteLayoutFlags(AbsoluteLayoutFlags.WidthProportional)
            .BackgroundColor = _themeService.GetColor(ColorDefinition.CircuitControllerBackground);

        _logger.LogDebug("Circuit controller started.");
    }

    /// <summary>
    /// Gets or sets the render core.
    /// </summary>
    /// <value>
    /// The render core.
    /// </value>
    public IRenderManager? RenderCore { get; set; }
}