using ACDCs.Interfaces.Drawing;

namespace ACDCs.Renderer.Drawings;

/// <summary>
/// Drawing element for a point.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="PointDrawing" /> class.
/// </remarks>
/// <seealso cref="Interfaces.Drawing.IDrawingTwoPoint" />
/// <seealso cref="Interfaces.Drawing.IDrawing" />
public class PointDrawing : IDrawing, IDrawingTwoPoint
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PointDrawing"/> class.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="x">The x.</param>
    /// <param name="y">The y.</param>
    /// <param name="x2">The x2.</param>
    /// <param name="y2">The y2.</param>
    /// <param name="isRelativeScale">if set to <c>true</c> [is relative scale].</param>
    public PointDrawing(string id, float x, float y, float x2, float y2, bool isRelativeScale = false)
    {
        Id = id;
        IsRelativeScale = isRelativeScale;
        X = x;
        X2 = x2;
        Y = y;
        Y2 = y2;
    }

    /// <summary>
    /// Gets or sets the color of the background.
    /// </summary>
    /// <value>
    /// The color of the background.
    /// </value>
    public Color? BackgroundColor { get; set; }

    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>
    /// The identifier.
    /// </value>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is relative scale.
    /// </summary>
    /// <value>
    /// <c>true</c> if this instance is relative scale; otherwise, <c>false</c>.
    /// </value>
    public bool IsRelativeScale { get; set; }

    /// <summary>
    /// Gets or sets the color of the line.
    /// </summary>
    /// <value>
    /// The color of the line.
    /// </value>
    public Color? LineColor { get; set; }

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
    public float X { get; set; }

    /// <summary>
    /// Gets or sets the x2.
    /// </summary>
    /// <value>
    /// The x2.
    /// </value>
    public float X2 { get; set; }

    /// <summary>
    /// Gets or sets the y.
    /// </summary>
    /// <value>
    /// The y.
    /// </value>
    public float Y { get; set; }

    /// <summary>
    /// Gets or sets the y2.
    /// </summary>
    /// <value>
    /// The y2.
    /// </value>
    public float Y2 { get; set; }
}