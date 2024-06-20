namespace ACDCs.Interfaces;

/// <summary>
/// The interface for the drawings.
/// </summary>
public interface IDrawing
{
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
    ///   <c>true</c> if this instance is relative scale; otherwise, <c>false</c>.
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
    public float Width { get; set; }

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