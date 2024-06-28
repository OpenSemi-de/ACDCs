using ACDCs.Interfaces.Drawing;

namespace ACDCs.Renderer.Drawings;

/// <summary>
/// Drawing element for a line.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="PointDrawing" /> class.
/// </remarks>
/// <seealso cref="ACDCs.Renderer.Drawings.BaseDrawing" />
/// <seealso cref="ACDCs.Interfaces.Drawing.IDrawingTwoPoint" />
/// <seealso cref="Interfaces.Drawing.IDrawing" />
public class LineDrawing : BaseDrawing, IDrawingTwoPoint
{
    /// <param name="id">The identifier.</param>
    /// <param name="x">The x.</param>
    /// <param name="y">The y.</param>
    /// <param name="x2">The x2.</param>
    /// <param name="y2">The y2.</param>
    /// <param name="isRelativeScale"></param>
    public LineDrawing(string id, float x, float y, float x2, float y2, bool isRelativeScale = false)
    {
        Id = id;
        IsRelativeScale = isRelativeScale;
        X = x;
        X2 = x2;
        Y = y;
        Y2 = y2;
    }

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