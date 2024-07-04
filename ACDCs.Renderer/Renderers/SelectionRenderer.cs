using ACDCs.Interfaces.Renderer;
using ACDCs.Renderer.Drawings;
using ACDCs.Renderer.Managers;
using ACDCs.Interfaces.Circuit;
using ACDCs.Structs;
using ACDCs.Interfaces;

namespace ACDCs.Renderer.Renderers;

/// <summary>
/// The selection renderer.
/// </summary>
/// <seealso cref="IRenderer" />
/// <seealso cref="ITextRenderer" />
public class SelectionRenderer : BaseRenderer<ArcDrawing>, IRenderer, ISelectionRenderer
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

        if (scene.ClickedBox == null)
        {
            return;
        }

        Quad quad = scene.ClickedBox.Quad;
        Color selectionColor = RenderSettingsManager.GetColor(ColorDefinition.Selection);
        canvas.StrokeColor = selectionColor;

        PathF path = new(quad.X1, quad.Y1);
        path.LineTo(quad.X2, quad.Y2);
        path.LineTo(quad.X3, quad.Y3);
        path.LineTo(quad.X4, quad.Y4);
        path.LineTo(quad.X1, quad.Y1);
        canvas.DrawPath(path);

        RenderSettingsManager.ApplyColors(canvas);
    }
}