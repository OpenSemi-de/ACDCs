namespace ACDCs.App.GUI.Modules;

using ACDCs.Interfaces;
using ACDCs.Renderer;
using ACDCs.Renderer.Components;
using ACDCs.Renderer.Drawings.Composite;
using ACDCs.Shared;
using ACDCs.Structs;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Layouts;
using Sharp.UI;

/// <summary>
/// The main circuit editor view for displaying the circuit renderer.
/// </summary>
/// <seealso cref="Sharp.UI.ContentView" />
/// <seealso cref="ACDCs.Interfaces.ICircuitEditorView" />
public class CircuitEditorView : ContentView, ICircuitEditorView
{
    private readonly ICircuitView _circuitView;
    private readonly ILogger _logger;
    private readonly IThemeService _themeService;

    /// <summary>
    /// Initializes a new instance of the <see cref="CircuitEditorView"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="themeService">The theme service.</param>
    public CircuitEditorView(ILogger logger, IThemeService themeService)
    {
        _logger = logger;
        _themeService = themeService;
        this.AbsoluteLayoutBounds(0, 30, 1, 1)
            .AbsoluteLayoutFlags(AbsoluteLayoutFlags.WidthProportional | AbsoluteLayoutFlags.HeightProportional)
            .BackgroundColor = _themeService.GetColor(ColorDefinition.CircuitBackground);

        _circuitView = ServiceHelper.GetService<ICircuitView>();
        Content = (View)_circuitView;
        _logger.LogDebug("Circuit editor started.");
    }

    /// <summary>
    /// Gets the render core.
    /// </summary>
    /// <value>
    /// The render core.
    /// </value>
    public IRenderManager? RenderCore => _circuitView.RenderCore;
}