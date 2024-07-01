using ACDCs.Interfaces.Circuit;
using ACDCs.Interfaces.Drawing;
using ACDCs.Shared;
using ACDCs.Structs;
using Rect = Microsoft.Maui.Graphics.Rect;

namespace ACDCs.Renderer.Renderers;

/// <summary>
/// Sub renderer class as base for renderers.
/// </summary>
public class BaseRenderer<T>
{
    private List<IDrawing> _drawings = [];

    private Point _position;

    private IScene? _scene;

    /// <summary>
    /// Gets or sets the drawings.
    /// </summary>
    /// <value>
    /// The drawings.
    /// </value>
    public List<IDrawing> Drawings { get => _drawings; set => _drawings = value; }

    /// <summary>
    /// Gets or sets the type of the drawing.
    /// </summary>
    /// <value>
    /// The type of the drawing.
    /// </value>
    public virtual Type DrawingType { get; } = typeof(T);

    /// <summary>
    /// Gets or sets the position.
    /// </summary>
    /// <value>
    /// The position.
    /// </value>
    public Point Position { get => _position; set => _position = value; }

    /// <summary>
    /// Gets or sets the scene.
    /// </summary>
    /// <value>
    /// The scene.
    /// </value>
    public IScene? Scene { get => _scene; set => _scene = value; }

    /// <summary>
    /// Gets the position and end.
    /// </summary>
    /// <param name="drawing">The drawing.</param>
    /// <param name="x">The x.</param>
    /// <param name="y">The y.</param>
    /// <param name="x2">The x2.</param>
    /// <param name="y2">The y2.</param>
    public Quad GetPositionAndEnd(IDrawing drawing, ref float x, ref float y, ref float x2, ref float y2)
    {
        if (drawing == null || drawing.ParentDrawing == null)
        {
            return new();
        }

        if (drawing.IsRelativeScale)
        {
            if (drawing.ParentDrawing is IDrawingWithSize drawingWithSize)
            {
                float? stepSize = Scene?.StepSize;
                x = Convert.ToSingle(drawing.ParentDrawing.X + Position.X + stepSize * drawing.X * drawingWithSize.Width);
                y = Convert.ToSingle(drawing.ParentDrawing.Y + Position.Y + stepSize * drawing.Y * drawingWithSize.Height);

                x2 = Convert.ToSingle(drawing.ParentDrawing.X + Position.X + stepSize * ((IDrawingTwoPoint)drawing).X2 * drawingWithSize.Width);
                y2 = Convert.ToSingle(drawing.ParentDrawing.Y + Position.Y + stepSize * ((IDrawingTwoPoint)drawing).Y2 * drawingWithSize.Height);

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
            x += Convert.ToSingle(Position.X);
            y += Convert.ToSingle(Position.Y);
            x2 += Convert.ToSingle(Position.X);
            y2 += Convert.ToSingle(Position.Y);
        }

        return RegisterClickBox(x, y, x2, y2, drawing.Rotation);
    }

    /// <summary>
    /// Gets the size of the position and.
    /// </summary>
    /// <param name="drawing">The drawing.</param>
    /// <param name="x">The x.</param>
    /// <param name="y">The y.</param>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    public Quad GetPositionAndSize(IDrawing drawing, ref float x, ref float y, ref float width, ref float height)
    {
        if (drawing.IsRelativeScale)
        {
            if (drawing.ParentDrawing == null)
            {
                return new();
            }

            if (drawing.ParentDrawing is IDrawingWithSize drawingWithSize)
            {
                float? stepSize = Scene?.StepSize;
                x = Convert.ToSingle(drawing.ParentDrawing.X + Position.X + (Scene?.StepSize * drawing.X * drawingWithSize.Width));
                y = Convert.ToSingle(drawing.ParentDrawing.Y + Position.Y + (Scene?.StepSize * drawing.Y * drawingWithSize.Height));
                width = Convert.ToSingle(drawingWithSize.Width * width * Scene?.StepSize);
                height = Convert.ToSingle(drawingWithSize.Height * height * Scene?.StepSize);

                if (drawing.ParentDrawing is ICompositeDrawing composite)
                {
                    x += Convert.ToSingle(composite.Offset.X * stepSize);
                    y += Convert.ToSingle(composite.Offset.Y * stepSize);
                }
            }
        }
        else
        {
            x += Convert.ToSingle(Position.X);
            y += Convert.ToSingle(Position.Y);
        }

        return RegisterClickBox(x, y, x + width, y + width, drawing.Rotation);
    }

    /// <summary>
    /// Sets the position.
    /// </summary>
    /// <param name="position">The position.</param>
    public void SetPosition(Point position)
    {
        Position = position;
    }

    /// <summary>
    /// Sets the scene.
    /// </summary>
    /// <param name="scene">The scene.</param>
    /// <param name="drawingType">Type of the drawing.</param>
    public void SetScene(IScene scene, Type drawingType)
    {
        Scene = scene;
        Drawings = scene
            .Drawings
            .Where(d => d.GetType() == drawingType)
            .ToList();

        Drawings.AddRange(
            scene
            .Drawings
            .Where(d => d is ICompositeDrawing)
            .SelectMany(d => ((ICompositeDrawing)d).GetDrawings())
            .Where(d => d.GetType() == drawingType)
            .ToList()
            );
    }

    private Quad RegisterClickBox(float x, float y, float x2, float y2, float rotation)
    {
        Quad quad = new(x, y, x2, y, x2, y2, x, y2);
        Scene?.ClickBoxes.Add(quad);
        return quad;
    }
}