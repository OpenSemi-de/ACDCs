﻿using ACDCs.Interfaces.Renderer;
using ACDCs.Interfaces.Circuit;
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
    /// <param name="scene">The scene.</param>
    /// <param name="canvas">The canvas.</param>
    /// <param name="dirtyRect">The rect to draw</param>
    public void Draw(IScene scene, ICanvas canvas, RectF dirtyRect)
    {
        RenderSettingsManager.ApplyColors(canvas);

        foreach (LineDrawing line in scene.Drawings.OfType<LineDrawing>())
        {
            float x = line.X;
            float y = line.Y;
            float x2 = line.X2;
            float y2 = line.Y2;

            BaseRendererHelper.GetPositionAndEnd(scene, Position, line, ref x, ref y, ref x2, ref y2);

            canvas.DrawLine(x, y, x2, y2);
        }
    }
}