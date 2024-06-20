using ACDCs.Interfaces;
using Microsoft.Extensions.Logging;
using Rect = ACDCs.Interfaces.Rect;

namespace ACDCs.Renderer.Managers;

/// <summary>
/// The rendering core class.
/// </summary>
/// <seealso cref="IRenderManager" />
/// <seealso cref="IDrawable" />
public class RenderManager : IRenderManager, IDrawable
{
    private readonly Color _backgroundColor;
    private readonly Color _fontColor;
    private readonly ILogger _logger;
    private readonly ISceneManager _sceneManager;
    private readonly Color _strokeColor;
    private readonly IThemeService _themeService;
    private Microsoft.Maui.Graphics.Point _position = new(100, 100);
    private int _stepSize = Convert.ToInt32(1000 / 25.4);

    /// <summary>
    /// Initializes a new instance of the <see cref="RenderManager" /> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="themeService">The theme service.</param>
    /// <param name="sceneManager">The scene manager.</param>
    public RenderManager(ILogger logger, IThemeService themeService, ISceneManager sceneManager)
    {
        _logger = logger;
        _themeService = themeService;
        _sceneManager = sceneManager;
        _backgroundColor = _themeService.GetColor(ColorDefinition.CircuitRendererBackground);
        _strokeColor = _themeService.GetColor(ColorDefinition.CircuitRendererStroke);
        _fontColor = _themeService.GetColor(ColorDefinition.CircuitRendererFont);

        _sceneManager.SetPosition(_position);

        _logger.LogDebug("Circuit renderer core started.");

#if DEBUG
        IsDebug = true;
#endif
    }

    /// <summary>
    /// Gets or sets the base square.
    /// </summary>
    /// <value>
    /// The base square.
    /// </value>
    public Rect BaseSquare { get; set; } = new Rect(0, 0, 1000, 1000);

    /// <summary>
    /// Gets a value indicating whether this instance has outline.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance has outline; otherwise, <c>false</c>.
    /// </value>
    public bool HasOutline { get; set; } = true;

    /// <summary>
    /// Gets a value indicating whether this instance is debug.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is debug; otherwise, <c>false</c>.
    /// </value>
    public bool IsDebug { get; set; }

    /// <summary>
    /// Gets the position.
    /// </summary>
    /// <value>
    /// The position.
    /// </value>
    public Microsoft.Maui.Graphics.Point Position { get => _position; set => _position = value; }

    /// <summary>
    /// Gets or sets the size of the step.
    /// </summary>
    /// <value>
    /// The size of the step.
    /// </value>
    public int StepSize { get => _stepSize; set => _stepSize = value; }

    /// <summary>
    /// Draws the specified canvas.
    /// </summary>
    /// <param name="canvas">The canvas.</param>
    /// <param name="dirtyRect">The dirty rect.</param>
    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        if (IsDebug)
        {
            _logger.LogDebug($"Circuit renderer drawing: {dirtyRect.ToJson()}");
        }

        SetColors(canvas);
        FillBackground(canvas, dirtyRect);
        DrawBaseSquare(canvas);
        _sceneManager.Draw(canvas);

        if (IsDebug)
        {
            DrawDebug(canvas);
        }
    }

    /// <summary>
    /// Loads the scene from json.
    /// </summary>
    /// <param name="jsonScene">The json scene.</param>
    public void LoadJson(string jsonScene)
    {
        _sceneManager.LoadJson(jsonScene);
    }

    /// <summary>
    /// Sets the position offset.
    /// </summary>
    /// <param name="x">The x.</param>
    /// <param name="y">The y.</param>
    public void SetPositionOffset(float x, float y)
    {
        _position.X += x;
        _position.Y += y;
        if (_position.X > 100) _position.X = 100;
        if (_position.Y > 100) _position.Y = 100;
        if (_position.X < -1 * (BaseSquare.X + BaseSquare.Width) + 100) _position.X = -1 * (BaseSquare.X + BaseSquare.Width) + 100;
        if (_position.Y < -1 * (BaseSquare.Y + BaseSquare.Height) + 100) _position.Y = -1 * (BaseSquare.Y + BaseSquare.Height) + 100;
        _sceneManager.SetPosition(_position);
    }

    private static void FillBackground(ICanvas canvas, RectF dirtyRect)
    {
        canvas.FillRectangle(dirtyRect.X, dirtyRect.Y, dirtyRect.Width, dirtyRect.Height);
    }

    private void DrawBaseSquare(ICanvas canvas)
    {
        if (HasOutline)
        {
            int startX = Convert.ToInt32(BaseSquare.X);
            int endX = Convert.ToInt32(BaseSquare.X + BaseSquare.Width);
            int startY = Convert.ToInt32(BaseSquare.Y);
            int endY = Convert.ToInt32(BaseSquare.Y + BaseSquare.Height);
            for (int x = startX; x < endX; x += _stepSize)
            {
                for (int y = startY; y < endY; y += _stepSize)
                {
                    canvas.DrawRectangle(Offset(new Rect(x, y, _stepSize, _stepSize)));
                }
            }
        }
    }

    private void DrawDebug(ICanvas canvas)
    {
        canvas.DrawString(_position.ToJson(), 0, 0, HorizontalAlignment.Left);
    }

    private Microsoft.Maui.Graphics.Rect Offset(Rect rect)
    {
        return rect.FromRect().Offset(_position.X, _position.Y);
    }

    private void SetColors(ICanvas canvas)
    {
        canvas.FillColor = _backgroundColor;
        canvas.FontColor = _fontColor;
        canvas.StrokeColor = _strokeColor;
    }
}