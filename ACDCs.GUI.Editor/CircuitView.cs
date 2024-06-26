﻿namespace ACDCs.App.GUI.Modules;

using ACDCs.Interfaces;
using ACDCs.Interfaces.Circuit;
using ACDCs.Interfaces.Renderer;
using ACDCs.Shared;
using ACDCs.Structs;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Layouts;
using Sharp.UI;

/// <summary>
/// The main circuit view for displaying the circuit renderer.
/// </summary>
/// <seealso cref="Sharp.UI.ContentView" />
/// <seealso cref="ACDCs.Interfaces.ICircuitEditorView" />
public class CircuitView : ContentView, ICircuitView
{
    private readonly ICircuitRenderer _circuitRenderer;
    private readonly ILogger _logger;
    private readonly IRenderManager _renderManager;
    private readonly IThemeService _themeService;

    /// <summary>
    /// Initializes a new instance of the <see cref="CircuitEditorView" /> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="themeService">The theme service.</param>
    public CircuitView(ILogger logger, IThemeService themeService)
    {
        _logger = logger;
        _themeService = themeService;
        this.AbsoluteLayoutBounds(0, 0, 1, 1)
            .AbsoluteLayoutFlags(AbsoluteLayoutFlags.All)
            .BackgroundColor = _themeService.GetColor(ColorDefinition.CircuitBackground);

        _circuitRenderer = ServiceHelper.GetService<ICircuitRenderer>();
        Content = (View)_circuitRenderer;
        _renderManager = _circuitRenderer.RenderManager;
        _logger.LogDebug("Circuit editor started.");
    }

    /// <summary>
    /// Gets the render core.
    /// </summary>
    /// <value>
    /// The render core.
    /// </value>
    public IRenderManager RenderCore => _renderManager;

    /// <summary>
    /// Adds a component.
    /// </summary>
    /// <param name="component">The component.</param>
    public void AddComponent(IComponent component)
    {
        _renderManager.AddComponent(component);
    }

    /// <summary>
    /// Loads the scene from json.
    /// </summary>
    /// <param name="jsonScene">The json scene.</param>
    public void LoadJson(string jsonScene)
    {
        _circuitRenderer.LoadJson(jsonScene);
    }

    /// <summary>
    /// Sets the scene.
    /// </summary>
    /// <param name="scene">The scene.</param>
    public void SetScene(IScene scene)
    {
        _renderManager.SetScene(scene);
    }
}