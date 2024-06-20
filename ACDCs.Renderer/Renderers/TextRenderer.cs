using ACDCs.Interfaces;
using ACDCs.Renderer.Drawings;
using ACDCs.Renderer.Managers;

namespace ACDCs.Renderer.Renderers;

/// <summary>
/// The text renderer.
/// </summary>
/// <seealso cref="IRenderer" />
/// <seealso cref="ITextRenderer" />
public class TextRenderer : SubRenderer<TextDrawing>, IRenderer, ITextRenderer
{
    /// <summary>
    /// Draws on the specified canvas.
    /// </summary>
    /// <param name="canvas">The canvas.</param>
    public void Draw(ICanvas canvas)
    {
        RenderSettingsManager.ApplyColors(canvas);

        foreach (TextDrawing text in Drawings)
        {
            float x = text.X;
            float y = text.Y;
            float width = text.Width;
            float height = text.Height;

            GetPositionAndSize(text, ref x, ref y, ref width, ref height);

            canvas.DrawString(text.Text, x, y, width, height, HorizontalAlignment.Center, VerticalAlignment.Center);
        }
    }
}