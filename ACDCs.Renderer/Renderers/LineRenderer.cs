using ACDCs.Interfaces.Renderer;
using ACDCs.Renderer.Drawings;
using ACDCs.Renderer.Managers;
using ACDCs.Structs;

namespace ACDCs.Renderer.Renderers;

/// <summary>
/// The line renderer.
/// </summary>
/// <seealso cref="IRenderer" />
/// <seealso cref="ITextRenderer" />
public class LineRenderer : BaseRenderer<LineDrawing>, IRenderer, ILineRenderer
{
    /// <summary>
    /// Draws on the specified canvas.
    /// </summary>
    /// <param name="canvas">The canvas.</param>
    public void Draw(ICanvas canvas)
    {
        RenderSettingsManager.ApplyColors(canvas);

        foreach (LineDrawing line in Drawings.Cast<LineDrawing>())
        {
            float x = line.X;
            float y = line.Y;
            float x2 = line.X2;
            float y2 = line.Y2;

            Quad clickBox = GetPositionAndEnd(line, ref x, ref y, ref x2, ref y2);

            canvas.DrawLine(x, y, x2, y2);

            if (Scene?.Debug.ShowClickBoxes ?? false)
            {
                PathF path = new(clickBox.X1, clickBox.Y1);
                path.LineTo(clickBox.X2, clickBox.Y2);
                path.LineTo(clickBox.X3, clickBox.Y3);
                path.LineTo(clickBox.X4, clickBox.Y4);
                path.LineTo(clickBox.X1, clickBox.Y1);

                canvas.DrawPath(path);
            }
        }
    }
}