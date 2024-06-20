using ACDCs.Interfaces;
using ACDCs.Renderer.Drawings;
using ACDCs.Renderer.Managers;
using Microsoft.Maui.Graphics;
using Rect = ACDCs.Interfaces.Rect;

namespace ACDCs.Renderer.Renderers;

/// <summary>
/// The line renderer.
/// </summary>
/// <seealso cref="ACDCs.Interfaces.IRenderer" />
/// <seealso cref="ACDCs.Interfaces.ITextRenderer" />
public class LineRenderer : SubRenderer<LineDrawing>, IRenderer, ILineRenderer
{
    /// <summary>
    /// Draws on the specified canvas.
    /// </summary>
    /// <param name="canvas">The canvas.</param>
    public void Draw(ICanvas canvas)
    {
        RenderSettingsManager.ApplyColors(canvas);
        canvas.StrokeSize = 2;

        foreach (LineDrawing line in Drawings)
        {
            float x = line.X;
            float y = line.Y;
            float x2 = line.X2;
            float y2 = line.Y2;

            GetPositionAndEnd(line, ref x, ref y, ref x2, ref y2);

            canvas.DrawLine(x, y, x2, y2);
        }
    }
}