using ACDCs.Interfaces.Renderer;
using ACDCs.Renderer.Drawings;
using ACDCs.Renderer.Managers;

namespace ACDCs.Renderer.Renderers;

/// <summary>
/// The point renderer.
/// </summary>
/// <seealso cref="ACDCs.Renderer.Renderers.SubRenderer&lt;ACDCs.Renderer.Drawings.PointDrawing&gt;" />
/// <seealso cref="Interfaces.Renderer.IRenderer" />
/// <seealso cref="Interfaces.Renderer.ILineRenderer" />
public class PointRenderer : SubRenderer<PointDrawing>, IRenderer, IPointRenderer
{
    /// <summary>
    /// Draws on the specified canvas.
    /// </summary>
    /// <param name="canvas">The canvas.</param>
    public void Draw(ICanvas canvas)
    {
        RenderSettingsManager.ApplyColors(canvas);
        canvas.StrokeSize = 2;

        foreach (PointDrawing point in Drawings)
        {
            float x = point.X;
            float y = point.Y;
            float x2 = point.X2;
            float y2 = point.Y2;

            GetPositionAndEnd(point, ref x, ref y, ref x2, ref y2);

            canvas.DrawEllipse(x, y, x2 - x, y2 - y);
        }
    }
}