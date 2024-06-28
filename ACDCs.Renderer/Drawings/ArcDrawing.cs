namespace ACDCs.Renderer.Drawings;

using ACDCs.Interfaces.Drawing;

/// <summary>
/// Drawing element for a arc.
/// </summary>
/// <seealso cref="ACDCs.Renderer.Drawings.BaseDrawing" />
/// <seealso cref="ACDCs.Interfaces.Drawing.IDrawingWithSize" />
public class ArcDrawing : BaseDrawing, IDrawingWithSize
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
    /// Gets or sets the height.
    /// </summary>
    /// <value>
    /// The height.
    /// </value>
    public float Height { get; set; }

    /// <summary>
    /// Gets the start angle.
    /// </summary>
    /// <value>
    /// The start angle.
    /// </value>
    public float StartAngle { get; }

    /// <summary>
    /// Gets the stop angle.
    /// </summary>
    /// <value>
    /// The stop angle.
    /// </value>
    public float StopAngle { get; }

    /// <summary>
    /// Gets or sets the width.
    /// </summary>
    /// <value>
    /// The width.
    /// </value>
    public float Width { get; set; }
}