using ACDCs.Interfaces.Renderer;
using ACDCs.Interfaces.Circuit;
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
    /// <param name="scene">The scene.</param>
    /// <param name="canvas">The canvas.</param>
    /// <param name="dirtyRect">The rect to draw</param>
    public void Draw(IScene scene, ICanvas canvas, RectF dirtyRect)
    {
        RenderSettingsManager.ApplyColors(canvas);
        canvas.StrokeSize = 2;

        foreach (PointDrawing point in scene.Drawings.OfType<PointDrawing>())
        {
            float x = point.X;
            float y = point.Y;
            float x2 = point.X2;
            float y2 = point.Y2;

            BaseRendererHelper.GetPositionAndEnd(scene, Position, point, ref x, ref y, ref x2, ref y2);

            canvas.FillCircle((x + x2) / 2, (y + y2) / 2, (x2 - x) / 2);
            canvas.DrawCircle((x + x2) / 2, (y + y2) / 2, (x2 - x) / 2);
        }
    }
}