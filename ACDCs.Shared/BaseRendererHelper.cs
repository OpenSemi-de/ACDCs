using ACDCs.Interfaces.Circuit;
using ACDCs.Interfaces.Drawing;

namespace ACDCs.Renderer.Renderers;

/// <summary>
/// Helper class for BaseRenderer.
/// </summary>
public static class BaseRendererHelper
{
    /// <summary>
    /// Gets the position and end.
    /// </summary>
    /// <param name="scene">The scene.</param>
    /// <param name="position">The position.</param>
    /// <param name="drawing">The drawing.</param>
    /// <param name="x">The x.</param>
    /// <param name="y">The y.</param>
    /// <param name="x2">The x2.</param>
    /// <param name="y2">The y2.</param>
    /// <returns></returns>
    public static void GetPositionAndEnd(IScene? scene, Point position, IDrawing drawing, ref float x, ref float y, ref float x2, ref float y2)
    {
        if (scene == null)
        {
            return;
        }

        if (drawing == null || drawing.ParentDrawing == null)
        {
            return;
        }

        if (drawing.IsRelativeScale)
        {
            if (drawing.ParentDrawing is IDrawingWithSize drawingWithSize)
            {
                float? stepSize = scene.StepSize;
                x = Convert.ToSingle(drawing.ParentDrawing.X + position.X + stepSize * drawing.X * drawingWithSize.Width);
                y = Convert.ToSingle(drawing.ParentDrawing.Y + position.Y + stepSize * drawing.Y * drawingWithSize.Height);

                x2 = Convert.ToSingle(drawing.ParentDrawing.X + position.X + stepSize * ((IDrawingTwoPoint)drawing).X2 * drawingWithSize.Width);
                y2 = Convert.ToSingle(drawing.ParentDrawing.Y + position.Y + stepSize * ((IDrawingTwoPoint)drawing).Y2 * drawingWithSize.Height);

                if (drawing.ParentDrawing is ICompositeDrawing composite)
                {
                    x += Convert.ToSingle(composite.Offset.X * stepSize);
                    y += Convert.ToSingle(composite.Offset.Y * stepSize);
                    x2 += Convert.ToSingle(composite.Offset.X * stepSize);
                    y2 += Convert.ToSingle(composite.Offset.Y * stepSize);
                }
            }
        }
        else
        {
            x += Convert.ToSingle(position.X);
            y += Convert.ToSingle(position.Y);
            x2 += Convert.ToSingle(position.X);
            y2 += Convert.ToSingle(position.Y);
        }
    }

    /// <summary>
    /// Gets the size of the position and.
    /// </summary>
    /// <param name="scene">The scene.</param>
    /// <param name="position">The position.</param>
    /// <param name="drawing">The drawing.</param>
    /// <param name="x">The x.</param>
    /// <param name="y">The y.</param>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    public static void GetPositionAndSize(IScene? scene, Point position, IDrawing drawing, ref float x, ref float y, ref float width, ref float height)
    {
        if (scene == null)
        {
            return;
        }

        if (drawing.IsRelativeScale)
        {
            if (drawing.ParentDrawing == null)
            {
                return;
            }

            if (drawing.ParentDrawing is IDrawingWithSize drawingWithSize)
            {
                float? stepSize = scene.StepSize;
                x = Convert.ToSingle(drawing.ParentDrawing.X + position.X + (scene.StepSize * drawing.X * drawingWithSize.Width));
                y = Convert.ToSingle(drawing.ParentDrawing.Y + position.Y + (scene.StepSize * drawing.Y * drawingWithSize.Height));
                width = Convert.ToSingle(drawingWithSize.Width * width * scene.StepSize);
                height = Convert.ToSingle(drawingWithSize.Height * height * scene.StepSize);

                if (drawing.ParentDrawing is ICompositeDrawing composite)
                {
                    x += Convert.ToSingle(composite.Offset.X * stepSize);
                    y += Convert.ToSingle(composite.Offset.Y * stepSize);
                }
            }
        }
        else
        {
            x += Convert.ToSingle(position.X);
            y += Convert.ToSingle(position.Y);
        }
    }
}