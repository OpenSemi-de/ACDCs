using ACDCs.Interfaces.Drawing;

namespace ACDCs.Renderer.Drawings;

/// <summary>
/// Drawing element for a line.
/// </summary>
/// <seealso cref="Interfaces.Drawing.IDrawing" />
/// <remarks>
/// Initializes a new instance of the <see cref="PointDrawing" /> class.
/// </remarks>
/// <param name="id">The identifier.</param>
/// <param name="x">The x.</param>
/// <param name="y">The y.</param>
/// <param name="x2">The x2.</param>
/// <param name="y2">The y2.</param>
/// <param name="isRelativeScale"></param>
public class LineDrawing(string id, float x, float y, float x2, float y2, bool isRelativeScale = false) : IDrawing, IDrawingTwoPoint
{
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
    public bool IsRelativeScale { get; set; } = isRelativeScale;

    /// <summary>
    /// Gets or sets the parent drawing.
    /// </summary>
    /// <value>
    /// The parent drawing.
    /// </value>
    public IDrawing? ParentDrawing { get; set; }

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