namespace ACDCs.Interfaces;

/// <summary>
/// The interface to sum up the different sub renderers.
/// </summary>
public interface IRenderer
{
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
    void SetScene(IScene scene);
}