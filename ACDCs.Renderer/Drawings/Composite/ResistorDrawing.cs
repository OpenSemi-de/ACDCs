using ACDCs.Interfaces.Drawing;
using Point = ACDCs.Interfaces.Point;

namespace ACDCs.Renderer.Drawings.Composite;

/// <summary>
/// Drawing element for a resisitor.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ResistorDrawing" /> class.
/// </remarks>
/// <seealso cref="ACDCs.Renderer.Drawings.BaseDrawing" />
/// <seealso cref="ACDCs.Interfaces.Drawing.ICompositeDrawing" />
/// <seealso cref="ACDCs.Interfaces.Drawing.IDrawingWithSize" />
/// <seealso cref="IDrawing" />
/// <seealso cref="ICompositeDrawing" />
public class ResistorDrawing : BaseDrawing, ICompositeDrawing, IDrawingWithSize
{
    /// <param name="id">The identifier.</param>
    /// <param name="value">The value.</param>
    /// <param name="x">The x.</param>
    /// <param name="y">The y.</param>
    /// <param name="rotation">The rotation.</param>
    public ResistorDrawing(string id, float value, float x, float y, float rotation)
    {
        Id = id;
        Rotation = rotation;
        Value = value;
        X = x;
        Y = y;
    }

    /// <summary>
    /// Gets or sets the height.
    /// </summary>
    /// <value>
    /// The height.
    /// </value>
    public float Height { get; set; } = 1;

    /// <summary>
    /// Gets or sets the offset.
    /// </summary>
    /// <value>
    /// The offset.
    /// </value>
    public Point Offset { get; set; } = new Point(0, -0.5);

    /// <summary>
    /// Gets or sets the width.
    /// </summary>
    /// <value>
    /// The width.
    /// </value>
    public float Width { get; set; } = 2;

    /// <summary>
    /// Gets the drawings.
    /// </summary>
    /// <returns></returns>
    public List<IDrawing> GetDrawings()
    {
        List<IDrawing> drawings =
        [
            new PointDrawing(Id + "_Pin1", 0f, 0.45f, 0.1f, 0.55f, true),
            new LineDrawing(Id + "_Line1", 0.1f, 0.5f, 0.25f, 0.5f, true),
            new BoxDrawing(Id + "_Box1", 0.25f, 0.3f, 0.5f, 0.4f, true),
            new LineDrawing(Id + "_Line2", 0.75f, 0.5f, 0.9f, 0.5f, true),
            new PointDrawing(Id + "_Pin2", 0.9f, 0.45f, 1f, 0.55f, true),
            new TextDrawing(Id + "_Text", Value.ToString(), 0f, 0.8f, 1f, 0.4f, 0, true)
        ];

        foreach (IDrawing drawing in drawings)
        {
            drawing.ParentDrawing = this;
        }

        return drawings;
    }
}