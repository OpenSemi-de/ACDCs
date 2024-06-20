using ACDCs.Interfaces;

namespace ACDCs.Renderer.Drawings;

/// <summary>
/// Drawing element for a point.
/// </summary>
/// <seealso cref="ACDCs.Interfaces.IDrawing" />
/// <remarks>
/// Initializes a new instance of the <see cref="PointDrawing"/> class.
/// </remarks>
/// <param name="id">The identifier.</param>
/// <param name="x">The x.</param>
/// <param name="y">The y.</param>
/// <param name="width">The width.</param>
/// <param name="height">The height.</param>
public class PointDrawing(string id, float x, float y, float width, float height) : IDrawing
{
    /// <summary>
    /// Gets or sets the height.
    /// </summary>
    /// <value>
    /// The height.
    /// </value>
    public float Height { get; set; } = height;

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
    /// Gets or sets the width.
    /// </summary>
    /// <value>
    /// The width.
    /// </value>
    public float Width { get; set; } = width;

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
}