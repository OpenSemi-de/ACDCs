namespace ACDCs.Interfaces;

/// <summary>
/// The interface to sum up the different sub renderers.
/// </summary>
public interface IRenderer
{
    /// <summary>
    /// Gets the type of the drawing.
    /// </summary>
    /// <value>
    /// The type of the drawing.
    /// </value>
    Type DrawingType { get; }

    /// <summary>
    /// Draws on the specified canvas.
    /// </summary>
    /// <param name="canvas">The canvas.</param>
    void Draw(ICanvas canvas);

    /// <summary>
    /// Sets the position.
    /// </summary>
    /// <param name="position">The position.</param>
    void SetPosition(Microsoft.Maui.Graphics.Point position);

    /// <summary>
    /// Sets the scene.
    /// </summary>
    /// <param name="scene">The scene.</param>
    /// <param name="drawingType">Type of the drawing.</param>
    void SetScene(IScene scene, Type drawingType);
}