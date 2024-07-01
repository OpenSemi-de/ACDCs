using ACDCs.Interfaces;
using Microsoft.Extensions.Logging;
using ACDCs.Renderer.Managers;
using ACDCs.Interfaces.Renderer;
using ACDCs.Interfaces.Circuit;
using ACDCs.Shared;

namespace ACDCs.Renderer;

/// <summary>
/// The renderer view for circuits.
/// </summary>
public class CircuitRenderer : GraphicsView, ICircuitRenderer
{
    private readonly ILogger _logger;
    private readonly PanGestureRecognizer _panGestrueRecognizer;
    private readonly PointerGestureRecognizer _pointerGestureRecognizer;
    private readonly IRenderManager _renderManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="CircuitRenderer" /> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="themeService">The theme service.</param>
    public CircuitRenderer(ILogger logger, IThemeService themeService)
    {
        _panGestrueRecognizer = new PanGestureRecognizer();
        _panGestrueRecognizer.PanUpdated += PanGestrueRecognizer_PanUpdated;
        GestureRecognizers.Add(_panGestrueRecognizer);

        _pointerGestureRecognizer = new PointerGestureRecognizer();
        _pointerGestureRecognizer.PointerPressed += PointerGestureRecognizer_PointerPressed;
        GestureRecognizers.Add(_pointerGestureRecognizer);

        _logger = logger;
        RenderSettingsManager.SetService(themeService);
        _renderManager = ServiceHelper.GetService<IRenderManager>();

        Drawable = (IDrawable)_renderManager;

        _logger.LogDebug("Circuit renderer view started.");
    }

    /// <summary>
    /// Gets the render core.
    /// </summary>
    /// <value>
    /// The render core.
    /// </value>
    public IRenderManager RenderManager => _renderManager;

    /// <summary>
    /// Disables the movement.
    /// </summary>
    public void DisableMovement()
    {
        GestureRecognizers.Clear();
    }

    /// <summary>
    /// Loads the scene from json.
    /// </summary>
    /// <param name="jsonScene">The json scene.</param>
    /// <exception cref="NotImplementedException"></exception>
    public void LoadJson(string jsonScene)
    {
        _renderManager.LoadJson(jsonScene);
    }

    /// <summary>
    /// Sets the scene.
    /// </summary>
    /// <param name="scene">The scene.</param>
    public void SetScene(IScene scene)
    {
        _renderManager.SetScene(scene);
    }

    private void PanGestrueRecognizer_PanUpdated(object? sender, PanUpdatedEventArgs e)
    {
        Point position = _renderManager.Position;
        switch (e.StatusType)
        {
            case GestureStatus.Running:
                float x = Convert.ToSingle(e.TotalX / 20);
                float y = Convert.ToSingle(e.TotalY / 20);
                _renderManager.SetPositionOffset(x, y);
                Invalidate();
                break;

            case GestureStatus.Completed or GestureStatus.Canceled:
                break;
        }
    }

    private void PointerGestureRecognizer_PointerPressed(object? sender, PointerEventArgs e)
    {
        _renderManager.GetTapped(e.GetPosition(this));
    }

    private void TapGestureRecognizer_Tapped(object? sender, TappedEventArgs e)
    {
    }
}