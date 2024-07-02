using ACDCs.Interfaces.Renderer;
using ACDCs.Renderer.Drawings;
using ACDCs.Renderer.Managers;
using ACDCs.Interfaces.Circuit;

namespace ACDCs.Renderer.Renderers;

/// <summary>
/// The arc renderer.
/// </summary>
/// <seealso cref="IRenderer" />
/// <seealso cref="ITextRenderer" />
public class BackgroundRenderer : BaseRenderer<ArcDrawing>, IRenderer, IBackgroundRenderer
{
    /// <summary>
    /// Draws on the specified canvas.
    /// </summary>
    /// <param name="scene">The scene.</param>
    /// <param name="canvas">The canvas.</param>
    /// <param name="dirtyRect">The rect to draw</param>
    public void Draw(IScene scene, ICanvas canvas, RectF dirtyRect)
    {
        RenderSettingsManager.ApplyColors(canvas);
        if (scene.BackgroundColor != Colors.Transparent)
        {
            canvas.FillColor = scene.BackgroundColor;
        }

        canvas.FillRectangle(dirtyRect.X, dirtyRect.Y, dirtyRect.Width, dirtyRect.Height);
    }
}