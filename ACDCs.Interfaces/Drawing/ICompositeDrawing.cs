namespace ACDCs.Interfaces.Drawing;

/// <summary>
/// The interface for a drawing containing multiple drawings.
/// </summary>
public interface ICompositeDrawing
{
    /// <summary>
    /// Gets or sets the offset.
    /// </summary>
    /// <value>
    /// The offset.
    /// </value>
    public Point Offset { get; set; }

    /// <summary>
    /// Gets the drawings.
    /// </summary>
    /// <returns></returns>
    public List<IDrawing> GetDrawings();
}