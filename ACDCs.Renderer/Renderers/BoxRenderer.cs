using ACDCs.Interfaces.Renderer;
using ACDCs.Interfaces.Circuit;
using ACDCs.Renderer.Drawings;
using ACDCs.Renderer.Managers;

namespace ACDCs.Renderer.Renderers;

/// <summary>
/// The box renderer.
/// </summary>
/// <seealso cref="IRenderer" />
/// <seealso cref="ITextRenderer" />
public class BoxRenderer : BaseRenderer<BoxDrawing>, IRenderer, IBoxRenderer
{
    /// <summary>
    /// Draws on the specified canvas.
    /// </summary>
    /// <param name="scene">The scene.</param>
    /// <param name="canvas">The canvas.</param>
    /// <param name="dirtyRect">The rect to draw</param>
    public void Draw(IScene scene, ICanvas canvas, RectF dirtyRect)
    {
        foreach (BoxDrawing box in scene.Drawings.OfType<BoxDrawing>())
        {
            RenderSettingsManager.ApplyColors(canvas, box);

            float x = box.X;
            float y = box.Y;
            float width = box.Width;
            float height = box.Height;

            BaseRendererHelper.GetPositionAndSize(scene, Position, box, ref x, ref y, ref width, ref height);

            canvas.FillRectangle(x, y, width, height);
            canvas.DrawRectangle(x, y, width, height);
        }
    }
}