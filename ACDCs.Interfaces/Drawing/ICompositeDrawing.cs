namespace ACDCs.Interfaces.Drawing;

/// <summary>
/// The interface for a drawing containing multiple drawings.
/// </summary>
public interface ICompositeDrawing
{
    /// <summary>
    /// Gets the drawings.
    /// </summary>
    /// <returns></returns>
    public List<IDrawing> GetDrawings();
}