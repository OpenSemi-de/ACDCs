using ACDCs.Interfaces;

namespace ACDCs.Renderer.Renderers;

/// <summary>
/// Sub renderer class as base for renderers.
/// </summary>
public class SubRenderer
{
    private List<IDrawing> _drawings = [];

    private Microsoft.Maui.Graphics.Point _position;

    private IScene? _scene;

    /// <summary>
    /// Gets or sets the drawings.
    /// </summary>
    /// <value>
    /// The drawings.
    /// </value>
    public List<IDrawing> Drawings { get => _drawings; set => _drawings = value; }

    /// <summary>
    /// Gets or sets the type of the drawing.
    /// </summary>
    /// <value>
    /// The type of the drawing.
    /// </value>
    public virtual Type DrawingType { get; } = typeof(SubRenderer);

    /// <summary>
    /// Gets or sets the position.
    /// </summary>
    /// <value>
    /// The position.
    /// </value>
    public Microsoft.Maui.Graphics.Point Position { get => _position; set => _position = value; }

    /// <summary>
    /// Gets or sets the scene.
    /// </summary>
    /// <value>
    /// The scene.
    /// </value>
    public IScene? Scene { get => _scene; set => _scene = value; }

    /// <summary>
    /// Sets the position.
    /// </summary>
    /// <param name="position">The position.</param>
    public void SetPosition(Microsoft.Maui.Graphics.Point position)
    {
        Position = position;
    }

    /// <summary>
    /// Sets the scene.
    /// </summary>
    /// <param name="scene">The scene.</param>
    /// <param name="drawingType">Type of the drawing.</param>
    public void SetScene(IScene scene, Type drawingType)
    {
        Scene = scene;
        Drawings = scene
            .Drawings
            .Where(d => d.GetType() == drawingType)
            .ToList();

        Drawings.AddRange(
            scene
            .Drawings
            .Where(d => d is ICompositeDrawing)
            .SelectMany(d => ((ICompositeDrawing)d).GetDrawings())
            .Where(d => d.GetType() == drawingType)
            .ToList()
            );
    }
}