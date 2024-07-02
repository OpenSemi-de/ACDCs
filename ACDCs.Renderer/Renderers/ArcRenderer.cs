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
public class ArcRenderer : BaseRenderer<ArcDrawing>, IRenderer, IArcRenderer
{
    /// <summary>
    /// Draws on the specified canvas.
    /// </summary>
    /// <param name="scene">The scene.</param>
    /// <param name="canvas">The canvas.</param>
    /// <param name="dirtyRect">The rect to draw</param>
    public void Draw(IScene scene, ICanvas canvas, RectF dirtyRect)
    {
        foreach (ArcDrawing arc in scene.Drawings.OfType<ArcDrawing>())
        {
            RenderSettingsManager.ApplyColors(canvas, arc);

            float x = arc.X;
            float y = arc.Y;
            float width = arc.Width;
            float height = arc.Height;

            BaseRendererHelper.GetPositionAndSize(scene, Position, arc, ref x, ref y, ref width, ref height);

            canvas.DrawArc(x, y, width, height, arc.StartAngle, arc.StopAngle, false, false);
        }
    }
}