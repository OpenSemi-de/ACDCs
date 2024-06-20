using ACDCs.Interfaces;

namespace ACDCs.Renderer.Drawings;

/// <summary>
/// Drawing element for a line.
/// </summary>
/// <seealso cref="ACDCs.Interfaces.IDrawing" />
/// <remarks>
/// Initializes a new instance of the <see cref="PointDrawing" /> class.
/// </remarks>
/// <param name="id">The identifier.</param>
/// <param name="x">The x.</param>
/// <param name="y">The y.</param>
/// <param name="x2">The x2.</param>
/// <param name="y2">The y2.</param>
public class LineDrawing(string id, float x, float y, float x2, float y2) : IDrawing
{
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
    public float Rotation { get; set; }

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    /// <value>
    /// The value.
    /// </value>
    public float Value { get; set; }

    /// <summary>
    /// Gets or sets the x.
    /// </summary>
    /// <value>
    /// The x.
    /// </value>
    public float X { get; set; } = x;

    /// <summary>
    /// Gets or sets the x2.
    /// </summary>
    /// <value>
    /// The x2.
    /// </value>
    public float X2 { get; set; } = x2;

    /// <summary>
    /// Gets or sets the y.
    /// </summary>
    /// <value>
    /// The y.
    /// </value>
    public float Y { get; set; } = y;

    /// <summary>
    /// Gets or sets the y2.
    /// </summary>
    /// <value>
    /// The y2.
    /// </value>
    public float Y2 { get; set; } = y2;
}