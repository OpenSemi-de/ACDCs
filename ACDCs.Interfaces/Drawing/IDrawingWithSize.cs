namespace ACDCs.Interfaces.Drawing;

/// <summary>
/// Interface for drawing with size.
/// </summary>
public interface IDrawingWithSize
{
    /// <summary>
    /// Gets or sets the height.
    /// </summary>
    /// <value>
    /// The height.
    /// </value>
    public float Height { get; set; }

    /// <summary>
    /// Gets or sets the width.
    /// </summary>
    /// <value>
    /// The width.
    /// </value>
    public float Width { get; set; }
}