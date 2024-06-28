using ACDCs.Interfaces;
using ACDCs.Interfaces.Drawing;
using ACDCs.Interfaces.Renderer;
using ACDCs.Renderer.Managers;
using Rect = ACDCs.Interfaces.Rect;

namespace ACDCs.Renderer.Renderers;

/// <summary>
/// The line renderer.
/// </summary>
/// <seealso cref="Interfaces.Renderer.IRenderer" />
/// <seealso cref="Interfaces.Renderer.ITextRenderer" />
public class GridRenderer : BaseRenderer<IGridDrawing>, IRenderer, IGridRenderer
{
    private readonly Color _gridColor = RenderSettingsManager.GetGridColor();

    /// <summary>
    /// Draws on the specified canvas.
    /// </summary>
    /// <param name="canvas">The canvas.</param>
    public void Draw(ICanvas canvas)
    {
        RenderSettingsManager.ApplyColors(canvas);
        canvas.StrokeColor = _gridColor;
        canvas.StrokeSize = 0.7f;

        if (Scene != null && Scene.HasOutline)
        {
            int startX = Convert.ToInt32(Scene?.SceneSize.X);
            int endX = Convert.ToInt32(Scene?.SceneSize.X + Scene?.SceneSize.Width);
            int startY = Convert.ToInt32(Scene?.SceneSize.Y);
            int endY = Convert.ToInt32(Scene?.SceneSize.Y + Scene?.SceneSize.Height);
            float stepSize = Scene?.StepSize ?? 0;
            for (float x = startX; x < endX; x += stepSize)
            {
                for (float y = startY; y < endY; y += stepSize)
                {
                    canvas.DrawRectangle(Offset(new Rect(x, y, stepSize, stepSize)));
                }
            }
        }
    }

    private Microsoft.Maui.Graphics.Rect Offset(Rect rect)
    {
        return rect.FromRect().Offset(Position.X, Position.Y);
    }
}