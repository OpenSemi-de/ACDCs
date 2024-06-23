using ACDCs.Interfaces.Drawing;

namespace ACDCs.Renderer.Drawings;

/// <summary>
/// Drawing element for text.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="TextDrawing" /> class.
/// </remarks>
/// <seealso cref="ACDCs.Interfaces.Drawing.IDrawing" />
/// <seealso cref="ACDCs.Interfaces.Drawing.IDrawingWithSize" />
public class TextDrawing : IDrawing, IDrawingWithSize
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
    public TextDrawing(string id, string text, float x, float y, float width, float height, float rotation, bool isRelativeScale = false)
    {
        Height = height;
        Id = id;
        IsRelativeScale = isRelativeScale;
        Rotation = rotation;
        Text = text;
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

    /// <summary>
    /// Gets or sets the text.
    /// </summary>
    /// <value>
    /// The text.
    /// </value>
    public string Text { get; set; }

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    /// <value>
    /// The value.
    /// </value>
    public float Value { get; set; } = 0;

    /// <summary>
    /// Gets or sets the width.
    /// </summary>
    /// <value>
    /// The width.
    /// </value>
    public float Width { get; set; }

    /// <summary>
    /// Gets the x.
    /// </summary>
    /// <value>
    /// The x.
    /// </value>
    public float X { get; set; }

    /// <summary>
    /// Gets the y.
    /// </summary>
    /// <value>
    /// The y.
    /// </value>
    public float Y { get; set; }
}