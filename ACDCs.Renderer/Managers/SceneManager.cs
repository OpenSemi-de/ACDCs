using ACDCs.Interfaces;
using ACDCs.Interfaces.Circuit;
using ACDCs.Interfaces.Drawing;
using ACDCs.Interfaces.Renderer;
using ACDCs.Shared;
using Microsoft.Extensions.Logging;

namespace ACDCs.Renderer.Managers;

/// <summary>
/// The scene manager to handle the sub renderers.
/// </summary>
/// <seealso cref="ISceneManager" />
/// <seealso cref="ISceneManager" />
public class SceneManager : ISceneManager
{
    private readonly ILogger _logger;
    private readonly IThemeService _themeService;
    private readonly List<IRenderer> renderers = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="SceneManager" /> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="themeService">The theme service.</param>
    public SceneManager(ILogger logger, IThemeService themeService)
    {
        _logger = logger;
        _themeService = themeService;
        renderers.Add(ServiceHelper.GetService<IGridRenderer>());
        renderers.Add(ServiceHelper.GetService<IPointRenderer>());
        renderers.Add(ServiceHelper.GetService<IBoxRenderer>());
        renderers.Add(ServiceHelper.GetService<IArcRenderer>());
        renderers.Add(ServiceHelper.GetService<ILineRenderer>());
        renderers.Add(ServiceHelper.GetService<ITextRenderer>());

        Scene = new Scene();
    }

    /// <summary>
    /// Gets or sets the scene.
    /// </summary>
    /// <value>
    /// The scene.
    /// </value>
    public IScene Scene { get; set; }

    /// <summary>
    /// Adds the component.
    /// </summary>
    /// <param name="component">The component.</param>
    public void AddComponent(IComponent component)
    {
        Scene.Circuit.Components.Add(component);
        ProvideScene(Scene);
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
    /// Gets the tapped.
    /// </summary>
    /// <param name="clickPoint">The click point.</param>
    public void GetTapped(Point? clickPoint)
    {
        if (clickPoint == null)
        {
            return;
        }

        Scene.ClickBoxes.FirstOrDefault(b => SelectionHelper.PointInRect(clickPoint.Value, b));
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
    public void SetPosition(Point position)
    {
        renderers.ForEach(r => r.SetPosition(position));
    }

    /// <summary>
    /// Sets the scene.
    /// </summary>
    /// <param name="scene">The scene.</param>
    public void SetScene(IScene scene)
    {
        Scene = scene;
        ProvideScene(Scene);
    }

    private static IDrawing? GetDrawing(IComponent component)
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
            IDrawing? item = SceneManager.GetDrawing(component);
            if (item != null)
            {
                scene.Drawings.Add(item);
            }
        }

        foreach (IRenderer renderer in renderers)
        {
            renderer.SetScene(scene, renderer.DrawingType);
        }
    }
}