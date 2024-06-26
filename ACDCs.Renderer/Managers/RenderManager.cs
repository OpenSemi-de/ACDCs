﻿using ACDCs.Interfaces;
using ACDCs.Interfaces.Circuit;
using ACDCs.Shared;
using ACDCs.Structs;
using Microsoft.Extensions.Logging;

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
    private Point _position = new(100, 100);
    private float _stepSize = 25.4f;

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
    public Point Position { get => _position; set => _position = value; }

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
        _sceneManager.AddComponent(component);
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

        SetColors(canvas);
        FillBackground(_sceneManager.Scene, canvas, dirtyRect);
        _sceneManager.Draw(canvas);

        if (IsDebug && _sceneManager.Scene.Debug.HasOutline)
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
        _sceneManager.GetTapped(clickPoint);
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

    /// <summary>
    /// Sets the scene.
    /// </summary>
    /// <param name="scene">The scene.</param>
    public void SetScene(IScene scene)
    {
        _sceneManager.SetScene(scene);
    }

    private void DrawDebug(ICanvas canvas)
    {
        canvas.DrawString(_position.ToJson(), 0, 0, HorizontalAlignment.Left);
    }

    private void FillBackground(IScene scene, ICanvas canvas, RectF dirtyRect)
    {
        if (scene.BackgroundColor != Colors.Transparent)
        {
            canvas.FillColor = scene.BackgroundColor;
        }

        canvas.FillRectangle(dirtyRect.X, dirtyRect.Y, dirtyRect.Width, dirtyRect.Height);

        if (scene.BackgroundColor != Colors.Transparent)
        {
            SetColors(canvas);
        }
    }

    private void SetColors(ICanvas canvas)
    {
        canvas.FillColor = _backgroundColor;
        canvas.FontColor = _fontColor;
        canvas.StrokeColor = _strokeColor;
    }
}