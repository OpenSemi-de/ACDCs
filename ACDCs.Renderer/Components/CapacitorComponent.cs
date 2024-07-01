using ACDCs.Interfaces.Circuit;
using ACDCs.Interfaces.Drawing;
using ACDCs.Renderer.Drawings.Composite;

namespace ACDCs.Renderer.Components;

/// <summary>
/// Circuit component for a resistor.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ResistorComponent" /> class.
/// </remarks>
/// <seealso cref="IComponent" />
public class CapacitorComponent : IComponent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CapacitorComponent"/> class.
    /// </summary>
    public CapacitorComponent()
    {
        Id = "Capacitor";
        Rotation = 0;
        IsPolar = true;
        Value = 4.7f;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CapacitorComponent"/> class.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="value">The value.</param>
    /// <param name="x">The x.</param>
    /// <param name="y">The y.</param>
    /// <param name="rotation">The rotation.</param>
    /// <param name="isPolar">if set to <c>true</c> [is polar].</param>
    public CapacitorComponent(string id, float value, float x, float y, float rotation, bool isPolar = true)
    {
        Id = id;
        Rotation = rotation;
        IsPolar = isPolar;
        Value = value;
        X = x;
        Y = y;
    }

    /// <summary>
    /// Gets or sets the component display order.
    /// </summary>
    /// <value>
    /// The component display order.
    /// </value>
    public int ComponentDisplayOrder { get; set; } = 2;

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
    /// Gets a value indicating whether this instance is polar.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is polar; otherwise, <c>false</c>.
    /// </value>
    public bool IsPolar { get; }

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
    /// The width
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

    /// <summary>
    /// Gets the drawing.
    /// </summary>
    /// <returns></returns>
    public IDrawing GetDrawing()
    {
        CapacitorDrawing drawing = new(Id, Value, X, Y, Rotation, IsPolar);
        return drawing;
    }
}