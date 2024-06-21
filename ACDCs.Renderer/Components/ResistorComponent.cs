using ACDCs.Interfaces.Circuit;
using ACDCs.Interfaces.Drawing;
using ACDCs.Renderer.Drawings;

namespace ACDCs.Renderer.Components;

/// <summary>
/// Circuit component for a resistor.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ResistorComponent" /> class.
/// </remarks>
/// <seealso cref="Interfaces.Circuit.IComponent" />
/// <param name="id">The identifier.</param>
/// <param name="value">The value.</param>
/// <param name="x">The x.</param>
/// <param name="y">The y.</param>
/// <param name="width">The width.</param>
/// <param name="height">The height.</param>
/// <param name="rotation">The rotation.</param>

public class ResistorComponent(string id, float value, float x, float y, float width, float height, float rotation) : IComponent
{
    /// <summary>
    /// Gets or sets the height.
    /// </summary>
    /// <value>
    /// The height.
    /// </value>
    public float Height { get; set; } = height;

    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>
    /// The identifier.
    /// </value>
    public string Id { get; set; } = id;

    /// <summary>
    /// Gets or sets the rotation.
    /// </summary>
    /// <value>
    /// The rotation.
    /// </value>
    public float Rotation { get; set; } = rotation;

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    /// <value>
    /// The value.
    /// </value>
    public float Value { get; set; } = value;

    /// <summary>
    /// The width
    /// </summary>
    /// <value>
    /// The width.
    /// </value>
    public float Width { get; set; } = width;

    /// <summary>
    /// Gets or sets the x.
    /// </summary>
    /// <value>
    /// The x.
    /// </value>
    public float X { get; set; } = x;

    /// <summary>
    /// Gets or sets the y.
    /// </summary>
    /// <value>
    /// The y.
    /// </value>
    public float Y { get; set; } = y;

    /// <summary>
    /// Gets the drawing.
    /// </summary>
    /// <returns></returns>
    public IDrawing GetDrawing()
    {
        ResistorDrawing drawing = new(Id, Value, X, Y, Rotation);
        return drawing;
    }
}