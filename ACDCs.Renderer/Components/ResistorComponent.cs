using ACDCs.Interfaces;
using ACDCs.Renderer.Drawings;

namespace ACDCs.Renderer.Components;

/// <summary>
/// Circuit component for a resistor.
/// </summary>
/// <seealso cref="ACDCs.Interfaces.IComponent" />
public class ResistorComponent : IComponent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ResistorComponent"/> class.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="value">The value.</param>
    /// <param name="x">The x.</param>
    /// <param name="y">The y.</param>
    public ResistorComponent(string id, float value, float x, float y)
    {
        Id = id;
        Value = value;
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
    /// Gets or sets the value.
    /// </summary>
    /// <value>
    /// The value.
    /// </value>
    public float Value { get; set; }

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

    /// <summary>
    /// Gets the drawing.
    /// </summary>
    /// <returns></returns>
    public IDrawing GetDrawing()
    {
        ResistorDrawing drawing = new ResistorDrawing(Id, Value, X, Y);
        return drawing;
    }
}