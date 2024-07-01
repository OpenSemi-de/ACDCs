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
    private readonly List<ICircuitRenderer> _circuitRenderer;
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

        _circuitRenderer = [];
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
            CircuitRenderer circuitRenderer = (CircuitRenderer)ServiceHelper.GetService<ICircuitRenderer>();
            circuitRenderer.DisableMovement();
            circuitRenderer.WidthRequest = 60;
            circuitRenderer.HeightRequest = 60;

            component.X = -80;
            component.Y = -80;
            Scene scene = new()
            {
                SceneSize = new Rect(0, 0, 60, 60),
            };

            scene.StepSize /= 5;

            scene.Circuit.Components.Add(component);
            scene.BackgroundColor = Colors.White;
            scene.HasOutline = false;
            circuitRenderer.SetScene(scene);

            _circuitRenderer.Add(circuitRenderer);
            Children.Add(circuitRenderer);
        }
    }
}