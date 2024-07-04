using ACDCs.Interfaces;
using ACDCs.Interfaces.Drawing;
using ACDCs.Interfaces.Circuit;
using ACDCs.Interfaces.Renderer;
using ACDCs.Shared;
using ACDCs.Structs;
using Microsoft.Extensions.Logging;

namespace ACDCs.Renderer.Managers;

/// <summary>
/// The rendering core class.
/// </summary>
/// <seealso cref="ACDCs.Interfaces.IRenderManager" />
/// <seealso cref="Microsoft.Maui.Graphics.IDrawable" />
/// <seealso cref="IRenderManager" />
/// <seealso cref="IDrawable" />
public class RenderManager : IRenderManager, IDrawable
{
    private readonly ILogger _logger;
    private readonly IThemeService _themeService;
    private readonly List<IRenderer> renderers = [];
    private Point _position = new(100, 100);
    private IScene _scene;
    private float _stepSize = 25.4f;

    /// <summary>
    /// Initializes a new instance of the <see cref="RenderManager" /> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="themeService">The theme service.</param>
    public RenderManager(ILogger logger, IThemeService themeService)
    {
        _logger = logger;
        _themeService = themeService;

        renderers.Add(ServiceHelper.GetService<IBackgroundRenderer>());
        renderers.Add(ServiceHelper.GetService<IGridRenderer>());
        renderers.Add(ServiceHelper.GetService<IPointRenderer>());
        renderers.Add(ServiceHelper.GetService<IBoxRenderer>());
        renderers.Add(ServiceHelper.GetService<IArcRenderer>());
        renderers.Add(ServiceHelper.GetService<ILineRenderer>());
        renderers.Add(ServiceHelper.GetService<ITextRenderer>());
        renderers.Add(ServiceHelper.GetService<IArcRenderer>());
        renderers.Add(ServiceHelper.GetService<IDebugRenderer>());
        renderers.Add(ServiceHelper.GetService<ISelectionRenderer>());

        SetPositionOffset(Convert.ToSingle(Position.X), Convert.ToSingle(Position.Y));
        _scene = new Scene();

        _logger.LogDebug("Circuit renderer core started.");

#if DEBUG
        IsDebug = true;
#endif
    }

    /// <inheritdoc/>
    public event EventHandler? OnInvalidate;

    /// <summary>
    /// Gets or sets the base square.
    /// </summary>
    /// <value>
    /// The base square.
    /// </value>
    public Rect BaseSquare { get; set; } = new Rect(0, 0, 1000, 1000);

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
    public Point Position { get => _position; set => _position = value; }

    /// <summary>
    /// Gets or sets the scene.
    /// </summary>
    /// <value>
    /// The scene.
    /// </value>
    public IScene Scene { get => _scene; set => _scene = value; }

    /// <summary>
    /// Gets or sets the size of the step.
    /// </summary>
    /// <value>
    /// The size of the step.
    /// </value>
    public float StepSize { get => _stepSize; set => _stepSize = value; }

    /// <summary>
    /// Adds the component.
    /// </summary>
    /// <param name="component">The component.</param>
    public void AddComponent(IComponent component)
    {
        _scene.Circuit.Components.Add(component);
        SetScene(_scene);
    }

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

        ProvideScene();
        foreach (IRenderer renderer in renderers)
        {
            renderer.Draw(_scene, canvas, dirtyRect);
        }

        if (IsDebug && _scene.Debug.DrawDebug)
        {
            DrawDebug(canvas);
        }
    }

    /// <summary>
    /// Gets the tapped.
    /// </summary>
    /// <param name="clickPoint"></param>
    public void GetTapped(Point? clickPoint)
    {
        if (clickPoint == null)
        {
            return;
        }

        IClickBox? clickedBox =
            Scene.
            ClickBoxes.
            FirstOrDefault(clickBox =>
                SelectionHelper.PointInRect(clickPoint.Value, clickBox.Quad)
                );

        Scene.ClickedBox = clickedBox;

        OnInvalidate?.Invoke(this, new());
    }

    /// <summary>
    /// Loads the scene from json.
    /// </summary>
    /// <param name="jsonScene">The json scene.</param>
    public void LoadJson(string jsonScene)
    {
        Scene? scene = jsonScene.ToObjectFromJson<Scene>();

        if (scene == null)
        {
            return;
        }

        SetScene(scene);
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
        renderers.ForEach(r => r.SetPosition(_position));
    }

    /// <summary>
    /// Sets the scene.
    /// </summary>
    /// <param name="scene">The scene.</param>
    public void SetScene(IScene scene)
    {
        _scene = scene;
        ProvideScene();
    }

    private void DrawDebug(ICanvas canvas)
    {
        canvas.DrawString(_position.ToJson(), 0, 0, HorizontalAlignment.Left);
    }

    private void GetDrawing(IComponent component)
    {
        if (component.GetDrawing() is not IDrawing drawing)
        {
            return;
        }

        if (drawing != null)
        {
            _scene.Drawings.Add(drawing);
        }

        if (drawing is ICompositeDrawing composite)
        {
            _scene.Drawings.AddRange(composite.GetDrawings());
            RegisterClickBox(composite, component);
        }
    }

    private void ProvideScene()
    {
        _scene.Drawings.Clear();
        _scene.ClickBoxes.Clear();

        foreach (IComponent component in _scene.Circuit.Components)
        {
            GetDrawing(component);
        }
    }

    private void RegisterClickBox(ICompositeDrawing composite, IComponent component)
    {
        if (composite is IDrawing drawing)
        {
            float x = drawing.X;
            float y = drawing.Y;
            float width = 0;
            float height = 0;

            if (composite is IDrawingTwoPoint dtp)
            {
                width = dtp.X2 - drawing.X;
                height = dtp.Y2 - drawing.Y;
            }

            if (composite is IDrawingWithSize dws)
            {
                width = dws.Width * _scene.StepSize;
                height = dws.Height * _scene.StepSize;
            }

            x += (Convert.ToSingle(composite.Offset.X) * _scene.StepSize);
            y += (Convert.ToSingle(composite.Offset.Y) * _scene.StepSize);

            x += Convert.ToSingle(_position.X);
            y += Convert.ToSingle(_position.Y);

            if (_scene.Debug.ShowClickBoxes)
            {
                _logger.LogDebug($"ClickBox: {x},{y},{width},{height}");
            }

            Quad quad = new(
                x, y,
                x + width, y,
                x + width, y + height,
                x, y + height
            );

            _scene.ClickBoxes.Add(new ClickBox(component, quad));
        }
    }
}