using ACDCs.Interfaces;
using Microsoft.Extensions.Logging;
using ACDCs.Renderer.Managers;
using ACDCs.Interfaces.Renderer;

namespace ACDCs.Renderer;

/// <summary>
/// The renderer view for circuits.
/// </summary>
public class CircuitRenderer : GraphicsView, ICircuitRenderer
{
    private readonly ILogger _logger;
    private readonly PanGestureRecognizer _panGestrueRecognizer;
    private readonly IRenderManager _renderCore;
    private readonly IThemeService _themeService;
    private Microsoft.Maui.Graphics.Point _lastPosition = new();

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

        _logger = logger;
        _themeService = themeService;
        RenderSettingsManager.SetService(themeService);
        _renderCore = ServiceHelper.GetService<IRenderManager>();

        Drawable = (IDrawable)_renderCore;

        _logger.LogDebug("Circuit renderer view started.");
    }

    /// <summary>
    /// Gets the render core.
    /// </summary>
    /// <value>
    /// The render core.
    /// </value>
    public IRenderManager RenderCore => _renderCore;

    /// <summary>
    /// Loads the scene from json.
    /// </summary>
    /// <param name="jsonScene">The json scene.</param>
    /// <exception cref="NotImplementedException"></exception>
    public void LoadJson(string jsonScene)
    {
        _renderCore.LoadJson(jsonScene);
    }

    private void PanGestrueRecognizer_PanUpdated(object? sender, PanUpdatedEventArgs e)
    {
        Microsoft.Maui.Graphics.Point position = _renderCore.Position;
        switch (e.StatusType)
        {
            case GestureStatus.Started:
                _lastPosition = position;
                break;

            case GestureStatus.Running:
                float x = Convert.ToSingle(e.TotalX / 20);
                float y = Convert.ToSingle(e.TotalY / 20);
                _renderCore.SetPositionOffset(x, y);
                Invalidate();
                break;

            case GestureStatus.Completed or GestureStatus.Canceled:
                break;
        }
    }
}