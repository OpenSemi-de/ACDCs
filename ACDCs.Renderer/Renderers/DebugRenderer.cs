using ACDCs.Interfaces.Drawing;
using ACDCs.Interfaces.Renderer;
using ACDCs.Interfaces.Circuit;
using ACDCs.Renderer.Managers;
using ACDCs.Structs;

namespace ACDCs.Renderer.Renderers;

/// <summary>
/// The debug renderer.
/// </summary>
/// <seealso cref="IRenderer" />
/// <seealso cref="ITextRenderer" />
public class DebugRenderer : BaseRenderer<IDrawing>, IRenderer, IDebugRenderer
{
    /// <summary>
    /// Draws on the specified canvas.
    /// </summary>
    /// <param name="scene">The scene.</param>
    /// <param name="canvas">The canvas.</param>
    /// <param name="dirtyRect">The rect to draw</param>
    public void Draw(IScene scene, ICanvas canvas, RectF dirtyRect)
    {
        if (scene == null)
        {
            return;
        }

        RenderSettingsManager.ApplyColors(canvas);

        if (scene.Debug.ShowClickBoxes)
        {
            foreach (Quad quad in scene.ClickBoxes)
            {
                PathF path = new(quad.X1, quad.Y1);
                path.LineTo(quad.X2, quad.Y2);
                path.LineTo(quad.X3, quad.Y3);
                path.LineTo(quad.X4, quad.Y4);
                path.LineTo(quad.X1, quad.Y1);
                canvas.DrawPath(path);
            }
        }
    }
}