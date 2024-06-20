using ACDCs.Interfaces;

namespace ACDCs.Renderer.Drawings;

/// <summary>
/// Drawing element for a box.
/// </summary>
/// <seealso cref="ACDCs.Interfaces.IDrawing" />
/// <seealso cref="IDrawing" />
public class BoxDrawing : IDrawing
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BoxDrawing"/> class.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="x">The x.</param>
    /// <param name="y">The y.</param>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    public BoxDrawing(string id, float x, float y, float width, float height)
    {
        Height = height;
        Id = id;
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
    /// Gets or sets the rotation.
    /// </summary>
    /// <value>
    /// The rotation.
    /// </value>
    public float Rotation { get; set; }

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    /// <value>
    /// The value.
    /// </value>
    public float Value { get; set; }

    /// <summary>
    /// Gets or sets the width.
    /// </summary>
    /// <value>
    /// The width.
    /// </value>
    public float Width { get; set; }

    /// <summary>
    /// Gets or sets the x.
    /// </summary>
    /// <value>
    /// The x.
    /// </value>
    public float X { get; set; }

    /// <summary>
    /// Gets or sets the y.
    /// </summary>
    /// <value>
    /// The y.
    /// </value>
    public float Y { get; set; }
}