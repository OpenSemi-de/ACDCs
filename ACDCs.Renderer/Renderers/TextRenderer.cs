using ACDCs.Interfaces.Circuit;
using ACDCs.Interfaces.Renderer;
using ACDCs.Renderer.Drawings;
using ACDCs.Renderer.Managers;

namespace ACDCs.Renderer.Renderers;

/// <summary>
/// The text renderer.
/// </summary>
/// <seealso cref="IRenderer" />
/// <seealso cref="ITextRenderer" />
public class TextRenderer : BaseRenderer<TextDrawing>, IRenderer, ITextRenderer
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

        foreach (TextDrawing text in scene.Drawings.OfType<TextDrawing>())
        {
            float x = text.X;
            float y = text.Y;
            float width = text.Width;
            float height = text.Height;

            BaseRendererHelper.GetPositionAndSize(scene, Position, text, ref x, ref y, ref width, ref height);

            canvas.DrawString(text.Text, x, y, width, height, HorizontalAlignment.Center, VerticalAlignment.Top);
        }
    }
}