using ACDCs.Interfaces.Circuit;

namespace ACDCs.Interfaces.Renderer;

/// <summary>
/// The interface to sum up the different sub renderers.
/// </summary>
public interface IRenderer
{
    /// <summary>
    /// Draws on the specified canvas.
    /// </summary>
    /// <param name="scene">The scene.</param>
    /// <param name="canvas">The canvas.</param>
    /// <param name="dirtyRect">The rect to draw</param>
    void Draw(IScene scene, ICanvas canvas, RectF dirtyRect);

    /// <summary>
    /// Sets the position.
    /// </summary>
    /// <param name="position">The position.</param>
    void SetPosition(Microsoft.Maui.Graphics.Point position);
}