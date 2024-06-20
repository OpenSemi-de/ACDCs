﻿using ACDCs.Interfaces;
using Microsoft.Extensions.Logging;

namespace ACDCs.Renderer.Managers;

/// <summary>
/// The scene manager to handle the sub renderers.
/// </summary>
/// <seealso cref="ISceneManager" />
public class SceneManager : ISceneManager
{
    private readonly ILogger _logger;
    private readonly List<IRenderer> renderers = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="SceneManager"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public SceneManager(ILogger logger)
    {
        _logger = logger;
        renderers.Add((IRenderer)ServiceHelper.GetService<ITextRenderer>());
    }

    /// <summary>
    /// Draws on the specified canvas.
    /// </summary>
    /// <param name="canvas">The canvas.</param>
    public void Draw(ICanvas canvas)
    {
        foreach (IRenderer renderer in renderers)
        {
            renderer.Draw(canvas);
        }
    }

    /// <summary>
    /// Loads the scene from a json string.
    /// </summary>
    /// <param name="jsonScene">The json scene string.</param>
    public void LoadJson(string jsonScene)
    {
        Scene? scene = jsonScene.ToObjectFromJson<Scene>();

        if (scene == null)
        {
            return;
        }

        ProvideScene(scene);
    }

    /// <summary>
    /// Sets the position.
    /// </summary>
    /// <param name="position">The position.</param>
    public void SetPosition(Microsoft.Maui.Graphics.Point position)
    {
        renderers.ForEach(r => r.SetPosition(position));
    }

    private IDrawing? GetDrawing(IComponent component)
    {
        if (component.GetDrawing() is not IDrawing drawing)
        {
            return default;
        }

        return drawing;
    }

    private void ProvideScene(IScene scene)
    {
        scene.Drawings.Clear();

        foreach (IComponent component in scene.Circuit.Components)
        {
            IDrawing? item = GetDrawing(component);
            if (item != null)
            {
                scene.Drawings.Add(item);
            }
        }

        foreach (IRenderer renderer in renderers)
        {
            renderer.SetScene(scene);
        }
    }
}