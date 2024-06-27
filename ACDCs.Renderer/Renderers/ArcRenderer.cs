using ACDCs.Interfaces.Renderer;
using ACDCs.Renderer.Drawings;
using ACDCs.Renderer.Managers;

namespace ACDCs.Renderer.Renderers;

/// <summary>
/// The arc renderer.
/// </summary>
/// <seealso cref="IRenderer" />
/// <seealso cref="ITextRenderer" />
public class ArcRenderer : SubRenderer<ArcDrawing>, IRenderer, IArcRenderer
{
    /// <summary>
    /// Draws on the specified canvas.
    /// </summary>
    /// <param name="canvas">The canvas.</param>
    public void Draw(ICanvas canvas)
    {
        foreach (ArcDrawing arc in Drawings.Cast<ArcDrawing>())
        {
            RenderSettingsManager.ApplyColors(canvas, arc);

            float x = arc.X;
            float y = arc.Y;
            float width = arc.Width;
            float height = arc.Height;

            GetPositionAndSize(arc, ref x, ref y, ref width, ref height);

            canvas.DrawArc(x, y, width, height, arc.StartAngle, arc.StopAngle, false, false);
        }
    }
}