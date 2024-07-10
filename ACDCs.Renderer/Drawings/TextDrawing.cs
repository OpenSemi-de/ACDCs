using ACDCs.Interfaces.Drawing;

namespace ACDCs.Renderer.Drawings;

/// <summary>
/// Drawing element for text.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="TextDrawing" /> class.
/// </remarks>
/// <seealso cref="BaseDrawing" />
/// <seealso cref="IDrawing" />
/// <seealso cref="IDrawingWithSize" />
public class TextDrawing : BaseDrawing, IDrawingWithSize, ICompositeDrawing
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TextDrawing"/> class.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="text">The text.</param>
    /// <param name="x">The x.</param>
    /// <param name="y">The y.</param>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    /// <param name="rotation">The rotation.</param>
    /// <param name="isRelativeScale">if set to <c>true</c> [is relative scale].</param>
    public TextDrawing(string id, string text, float x, float y, float width, float height, float rotation, bool isRelativeScale = true)
    {
        Height = height;
        Id = id;
        IsRelativeScale = isRelativeScale;
        Rotation = rotation;
        Text = text;
        Width = width;
        X = x;
        Y = y;
        ParentDrawing = this;
    }

    /// <summary>
    /// Gets or sets the height.
    /// </summary>
    /// <value>
    /// The height.
    /// </value>
    public float Height { get; set; } = 1;

    /// <summary>
    /// Gets or sets the offset.
    /// </summary>
    /// <value>
    /// The offset.
    /// </value>
    public Point Offset { get; set; }

    /// <summary>
    /// Gets or sets the text.
    /// </summary>
    /// <value>
    /// The text.
    /// </value>
    public string Text { get; set; }

    /// <summary>
    /// Gets or sets the width.
    /// </summary>
    /// <value>
    /// The width.
    /// </value>
    public float Width { get; set; } = 2;

    /// <summary>
    /// Gets the drawings.
    /// </summary>
    /// <returns></returns>
    public List<IDrawing> GetDrawings()
    {
        return [this];
    }
}