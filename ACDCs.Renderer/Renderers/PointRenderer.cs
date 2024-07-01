using ACDCs.Interfaces.Renderer;
using ACDCs.Renderer.Drawings;
using ACDCs.Renderer.Managers;

namespace ACDCs.Renderer.Renderers;

/// <summary>
/// The point renderer.
/// </summary>
/// <seealso cref="IRenderer" />
/// <seealso cref="ILineRenderer" />
public class PointRenderer : BaseRenderer<PointDrawing>, IRenderer, IPointRenderer
{
    /// <summary>
    /// Draws on the specified canvas.
    /// </summary>
    /// <param name="canvas">The canvas.</param>
    public void Draw(ICanvas canvas)
    {
        RenderSettingsManager.ApplyColors(canvas);
        canvas.StrokeSize = 2;

        foreach (PointDrawing point in Drawings.Cast<PointDrawing>())
        {
            float x = point.X;
            float y = point.Y;
            float x2 = point.X2;
            float y2 = point.Y2;

            GetPositionAndEnd(point, ref x, ref y, ref x2, ref y2);

            canvas.FillCircle((x + x2) / 2, (y + y2) / 2, (x2 - x) / 2);
            canvas.DrawCircle((x + x2) / 2, (y + y2) / 2, (x2 - x) / 2);
        }
    }
}