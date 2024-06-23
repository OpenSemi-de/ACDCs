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
        RenderSettingsManager.ApplyColors(canvas);

        foreach (BoxDrawing text in Drawings.Cast<BoxDrawing>())
        {
            float x = text.X;
            float y = text.Y;
            float width = text.Width;
            float height = text.Height;

            GetPositionAndSize(text, ref x, ref y, ref width, ref height);

            canvas.FillRectangle(x, y, width, height);
            canvas.DrawRectangle(x, y, width, height);
        }
    }
}