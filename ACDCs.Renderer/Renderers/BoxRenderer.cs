using ACDCs.Interfaces.Renderer;
using ACDCs.Renderer.Drawings;
using ACDCs.Renderer.Managers;

namespace ACDCs.Renderer.Renderers;

/// <summary>
/// The box renderer.
/// </summary>
/// <seealso cref="IRenderer" />
/// <seealso cref="ITextRenderer" />
public class BoxRenderer : SubRenderer<BoxDrawing>, IRenderer, IBoxRenderer
{
    /// <summary>
    /// Draws on the specified canvas.
    /// </summary>
    /// <param name="canvas">The canvas.</param>
    public void Draw(ICanvas canvas)
    {
        foreach (BoxDrawing box in Drawings.Cast<BoxDrawing>())
        {
            RenderSettingsManager.ApplyColors(canvas, box);

            float x = box.X;
            float y = box.Y;
            float width = box.Width;
            float height = box.Height;

            GetPositionAndSize(box, ref x, ref y, ref width, ref height);

            canvas.FillRectangle(x, y, width, height);
            canvas.DrawRectangle(x, y, width, height);
        }
    }
}