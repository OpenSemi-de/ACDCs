using ACDCs.Interfaces.Drawing;

namespace ACDCs.Renderer.Drawings;

/// <summary>
/// Drawing element for a resisitor.
/// </summary>
/// <seealso cref="Interfaces.Drawing.IDrawing" />
/// <seealso cref="Interfaces.Drawing.ICompositeDrawing" />
/// <remarks>
/// Initializes a new instance of the <see cref="ResistorDrawing" /> class.
/// </remarks>
/// <param name="id">The identifier.</param>
/// <param name="value">The value.</param>
/// <param name="x">The x.</param>
/// <param name="y">The y.</param>
/// <param name="rotation">The rotation.</param>
public class ResistorDrawing(string id, float value, float x, float y, float rotation) : IDrawing, ICompositeDrawing, IDrawingWithSize
{
    /// <summary>
    /// Gets or sets the height.
    /// </summary>
    /// <value>
    /// The height.
    /// </value>
    public float Height { get; set; } = 3;

    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>
    /// The identifier.
    /// </value>
    public string Id { get; set; } = id;

    /// <summary>
    /// Gets or sets a value indicating whether this instance is relative scale.
    /// </summary>
    /// <value>
    /// <c>true</c> if this instance is relative scale; otherwise, <c>false</c>.
    /// </value>
    public bool IsRelativeScale { get; set; }

    /// <summary>
    /// Gets or sets the parent drawing.
    /// </summary>
    /// <value>
    /// The parent drawing.
    /// </value>
    public IDrawing ParentDrawing { get; set; }

    /// <summary>
    /// Gets or sets the rotation.
    /// </summary>
    /// <value>
    /// The rotation.
    /// </value>
    public float Rotation { get; set; } = rotation;

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    /// <value>
    /// The value.
    /// </value>
    public float Value { get; set; } = value;

    /// <summary>
    /// Gets or sets the width.
    /// </summary>
    /// <value>
    /// The width.
    /// </value>
    public float Width { get; set; } = 3;

    /// <summary>
    /// Gets or sets the x.
    /// </summary>
    /// <value>
    /// The x.
    /// </value>
    public float X { get; set; } = x;

    /// <summary>
    /// Gets or sets the y.
    /// </summary>
    /// <value>
    /// The y.
    /// </value>
    public float Y { get; set; } = y;

    /// <summary>
    /// Gets the drawings.
    /// </summary>
    /// <returns></returns>
    public List<IDrawing> GetDrawings()
    {
        List<IDrawing> drawings =
        [
            new PointDrawing(Id + "_Pin1", 0f, 0.4f, 0.2f, 0.6f, true),
            new LineDrawing(Id + "Line1", 0f, 0.5f, 0.2f, 0.5f, true),
            new BoxDrawing(Id + "Box1", 0.2f, 0.3f, 0.8f, 0.7f, true),
            new LineDrawing(Id + "Line2", 0.8f, 0.5f, 1f, 0.5f, true),
            new PointDrawing(Id + "_Pin2", 0.8f, 0.4f, 01f, 0.6f, true),
            new TextDrawing(Id + "_Text", Value.ToString(), 0f, 0.6f, 1f, 0.4f, 0, true)
        ];

        foreach (IDrawing drawing in drawings)
        {
            drawing.ParentDrawing = this;
        }

        return drawings;
    }
}