using ACDCs.Interfaces.Drawing;
using ACDCs.Interfaces.Renderer;
using ACDCs.Interfaces.Circuit;
using ACDCs.Renderer.Managers;
using ACDCs.Shared;

namespace ACDCs.Renderer.Renderers;

/// <summary>
/// The line renderer.
/// </summary>
/// <seealso cref="IRenderer" />
/// <seealso cref="ITextRenderer" />
public class GridRenderer : BaseRenderer<IGridDrawing>, IRenderer, IGridRenderer
{
    private readonly Color _gridColor = RenderSettingsManager.GetGridColor();

    /// <summary>
    /// Draws on the specified canvas.
    /// </summary>
    /// <param name="scene">The scene.</param>
    /// <param name="canvas">The canvas.</param>
    /// <param name="dirtyRect">The rect to draw</param>
    public void Draw(IScene scene, ICanvas canvas, RectF dirtyRect)
    {
        RenderSettingsManager.ApplyColors(canvas);
        canvas.StrokeColor = _gridColor;
        canvas.StrokeSize = 0.7f;

        if (scene != null && scene.HasOutline)
        {
            int startX = Convert.ToInt32(scene?.SceneSize.X);
            int endX = Convert.ToInt32(scene?.SceneSize.X + scene?.SceneSize.Width);
            int startY = Convert.ToInt32(scene?.SceneSize.Y);
            int endY = Convert.ToInt32(scene?.SceneSize.Y + scene?.SceneSize.Height);
            float stepSize = scene?.StepSize ?? 0;
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