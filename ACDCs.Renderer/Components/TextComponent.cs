using ACDCs.Interfaces;
using ACDCs.Renderer.Drawings;

namespace ACDCs.Renderer.Components;

/// <summary>
/// Circuit component for text.
/// </summary>
/// <seealso cref="ACDCs.Interfaces.IComponent" />
public class TextComponent : IComponent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TextComponent"/> class.
    /// </summary>
    public TextComponent()
    {
        Id = "";
        Text = "";
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TextComponent" /> class.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="text">The text.</param>
    /// <param name="x">The x.</param>
    /// <param name="y">The y.</param>
    public TextComponent(string id, string text, int x, int y)
    {
        Id = id;
        Text = text;
        X = x;
        Y = y;
    }

    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>
    /// The identifier.
    /// </value>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the rotation.
    /// </summary>
    /// <value>
    /// The rotation.
    /// </value>
    public float Rotation { get; set; }

    /// <summary>
    /// Gets the text.
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
    public float Value { get; set; }

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

    /// <summary>
    /// Gets the drawing.
    /// </summary>
    /// <returns></returns>
    public IDrawing GetDrawing()
    {
        return new TextDrawing(Id, Text, X, Y);
    }
}