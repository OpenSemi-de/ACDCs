namespace ACDCs.App.GUI.Modules;

using ACDCs.Interfaces;
using ACDCs.Interfaces.Circuit;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Layouts;
using Sharp.UI;
using ACDCs.Renderer;
using ACDCs.Interfaces.Renderer;
using ACDCs.Shared;
using ACDCs.Structs;

/// <summary>
/// The view for the circuit components.
/// </summary>
/// <seealso cref="ACDCs.Interfaces.ICircuitComponentView" />
/// <seealso cref="Sharp.UI.ContentView" />
public class CircuitComponentView : HorizontalStackLayout, ICircuitComponentView
{
    private readonly List<RendererButton> _circuitRenderers;
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
        this.AbsoluteLayoutBounds(0, 1, 1, 60)
            .AbsoluteLayoutFlags(AbsoluteLayoutFlags.YProportional | AbsoluteLayoutFlags.WidthProportional)
            .BackgroundColor = _themeService.GetColor(ColorDefinition.CircuitComponentBackground);

        _circuitRenderers = [];
        GenerateCircuitViews();

        _logger.LogDebug("Circuit components started.");
    }

    /// <summary>
    /// Gets or sets the render core.
    /// </summary>
    /// <value>
    /// The render core.
    /// </value>
    public IRenderManager? RenderCore { get; set; }

    private void Button_OnClicked(object? sender, EventArgs e)
    {
        _circuitRenderers.ForEach(r => r.Reset());
    }

    private void CircuitRendererPointerGestureRecognizer_PointerPressed(object? sender, PointerEventArgs e)
    {
    }

    private void GenerateCircuitViews()
    {
        List<IComponent> components = [];

        var types = AppDomain.
            CurrentDomain.
            GetAssemblies().
            SelectMany(
                a =>
                a.DefinedTypes.
                Where(
                    t =>
                    t.
                    ImplementedInterfaces.
                    Contains(typeof(IComponent))
                    )
                );

        foreach (var type in types)
        {
            if (Activator.CreateInstance(type) is IComponent component)
            {
                components.Add(component);
            }
        }

        components = [.. components.OrderBy(c => c.ComponentDisplayOrder)];

        foreach (var component in components)
        {
            RendererButton button = new RendererButton(component, _themeService);

            button.OnClicked += Button_OnClicked;
            _circuitRenderers.Add(button);

            Children.Add(button);
        }
    }
}