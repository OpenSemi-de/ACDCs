using ACDCs.Interfaces.Circuit;
using ACDCs.Interfaces.Drawing;

namespace ACDCs.Renderer.Renderers;

/// <summary>
/// Sub renderer class as base for renderers.
/// </summary>
public class SubRenderer<T>
{
    private List<IDrawing> _drawings = [];

    private Microsoft.Maui.Graphics.Point _position;

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
    public Microsoft.Maui.Graphics.Point Position { get => _position; set => _position = value; }

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
    public void GetPositionAndEnd(IDrawing drawing, ref float x, ref float y, ref float x2, ref float y2)
    {
        if (drawing.IsRelativeScale)
        {
            float? stepSize = Scene?.StepSize;
            x = Convert.ToSingle(drawing.ParentDrawing.X + Position.X + stepSize * drawing.X);
            y = Convert.ToSingle(drawing.ParentDrawing.Y + Position.Y + stepSize * drawing.Y);

            x2 = Convert.ToSingle(drawing.ParentDrawing.X + Position.X + stepSize * ((IDrawingTwoPoint)drawing).X2);
            y2 = Convert.ToSingle(drawing.ParentDrawing.Y + Position.Y + stepSize * ((IDrawingTwoPoint)drawing).Y2);
        }
        else
        {
            x += Convert.ToSingle(Position.X);
            y += Convert.ToSingle(Position.Y);
            x2 += Convert.ToSingle(Position.X);
            y2 += Convert.ToSingle(Position.Y);
        }
    }

    /// <summary>
    /// Gets the size of the position and.
    /// </summary>
    /// <param name="drawing">The drawing.</param>
    /// <param name="x">The x.</param>
    /// <param name="y">The y.</param>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    public void GetPositionAndSize(IDrawing drawing, ref float x, ref float y, ref float width, ref float height)
    {
        if (drawing.IsRelativeScale)
        {
            x = Convert.ToSingle(drawing.ParentDrawing.X + Position.X + (Scene?.StepSize * drawing.X));
            y = Convert.ToSingle(drawing.ParentDrawing.Y + Position.Y + (Scene?.StepSize * drawing.Y));
            width = Convert.ToSingle(drawing.ParentDrawing.Width * Scene?.StepSize);
            height = Convert.ToSingle(height * Scene?.StepSize);
        }
        else
        {
            x += Convert.ToSingle(Position.X);
            y += Convert.ToSingle(Position.Y);
        }
    }

    /// <summary>
    /// Sets the position.
    /// </summary>
    /// <param name="position">The position.</param>
    public void SetPosition(Microsoft.Maui.Graphics.Point position)
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
}