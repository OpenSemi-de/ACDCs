namespace ACDCs.Interfaces.Drawing;

/// <summary>
/// Interface for drawings woth two points (X2, Y2).
/// </summary>
public interface IDrawingTwoPoint
{
    /// <summary>
    /// Gets or sets the x2.
    /// </summary>
    /// <value>
    /// The x2.
    /// </value>
    public float X2 { get; set; }

    /// <summary>
    /// Gets or sets the y2.
    /// </summary>
    /// <value>
    /// The y2.
    /// </value>
    public float Y2 { get; set; }
}