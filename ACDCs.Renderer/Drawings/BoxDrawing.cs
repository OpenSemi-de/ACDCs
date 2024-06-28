using ACDCs.Interfaces.Drawing;

namespace ACDCs.Renderer.Drawings;

/// <summary>
/// Drawing element for a box.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="BoxDrawing" /> class.
/// </remarks>
/// <seealso cref="ACDCs.Renderer.Drawings.BaseDrawing" />
/// <seealso cref="ACDCs.Interfaces.Drawing.IDrawingWithSize" />
/// <seealso cref="Interfaces.Drawing.IDrawing" />
/// <seealso cref="IDrawing" />
public class BoxDrawing : BaseDrawing, IDrawingWithSize
{
    /// <param name="id">The identifier.</param>
    /// <param name="x">The x.</param>
    /// <param name="y">The y.</param>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    /// <param name="isRelativeScale"></param>
    public BoxDrawing(string id, float x, float y, float width, float height, bool isRelativeScale = false)
    {
        Height = height;
        Id = id;
        IsRelativeScale = isRelativeScale;
        Width = width;
        X = x;
        Y = y;
    }

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