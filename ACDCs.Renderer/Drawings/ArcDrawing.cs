namespace ACDCs.Renderer.Drawings;

using ACDCs.Interfaces.Drawing;

/// <summary>
/// Drawing element for a arc.
/// </summary>
public class ArcDrawing : IDrawing, IDrawingWithSize
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ArcDrawing" /> class.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="x">The x.</param>
    /// <param name="y">The y.</param>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    /// <param name="startAngle">The start angle.</param>
    /// <param name="stopAngle">The stop angle.</param>
    /// <param name="isRelativeScale">if set to <c>true</c> [is relative scale].</param>
    public ArcDrawing(string id, float x, float y, float width, float height, float startAngle, float stopAngle, bool isRelativeScale = false)
    {
        Id = id;
        X = x;
        Y = y;
        Width = width;
        Height = height;
        StartAngle = startAngle;
        StopAngle = stopAngle;
        IsRelativeScale = isRelativeScale;
    }

    /// <summary>
    /// Gets or sets the color of the background.
    /// </summary>
    /// <value>
    /// The color of the background.
    /// </value>
    public Color? BackgroundColor { get; set; }

    /// <summary>
    /// Gets or sets the height.
    /// </summary>
    /// <value>
    /// The height.
    /// </value>
    public float Height { get; set; }

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

    public float StartAngle { get; }
    public float StopAngle { get; }

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
    public float Width { get; set; }

    /// <summary>
    /// Gets or sets the x.
    /// </summary>
    /// <value>
    /// The x.
    /// </value>
    public float X { get; set; }

    /// <summary>
    /// Gets or sets the y.
    /// </summary>
    /// <value>
    /// The y.
    /// </value>
    public float Y { get; set; }
}