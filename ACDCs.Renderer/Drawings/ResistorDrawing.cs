using ACDCs.Interfaces;

namespace ACDCs.Renderer.Drawings;

/// <summary>
/// Drawing element for a resisitor.
/// </summary>
/// <seealso cref="ACDCs.Interfaces.IDrawing" />
/// <seealso cref="ACDCs.Interfaces.ICompositeDrawing" />
/// <remarks>
/// Initializes a new instance of the <see cref="ResistorDrawing" /> class.
/// </remarks>
/// <param name="id">The identifier.</param>
/// <param name="value">The value.</param>
/// <param name="x">The x.</param>
/// <param name="y">The y.</param>
/// <param name="rotation">The rotation.</param>
public class ResistorDrawing(string id, float value, float x, float y, float rotation) : IDrawing, ICompositeDrawing
{
    /// <summary>
    /// Gets or sets the height.
    /// </summary>
    /// <value>
    /// The height.
    /// </value>
    public float Height { get; set; } = 1;

    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>
    /// The identifier.
    /// </value>
    public string Id { get; set; } = id;

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
    public float Width { get; set; } = 1;

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
            new PointDrawing(Id + "_Pin1", 0f, 0.5f, 0.1f, 0.1f),
            new LineDrawing(Id + "Line1", 0f, 0.5f, 0.2f, 0.5f),
            new BoxDrawing(Id + "Box1", 0.2f, 0.3f, 0.8f, 0.7f),
            new LineDrawing(Id + "Line2", 0.8f, 0.5f, 1f, 0.5f),
            new PointDrawing(Id + "_Pin2", 1f, 0.5f, 0.1f, 0.1f),
        ];

        return drawings;
    }
}