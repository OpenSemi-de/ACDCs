using ACDCs.Structs;

namespace ACDCs.Interfaces.Circuit;

/// <summary>
/// Interface for the click box class,
/// </summary>
public interface IClickBox
{
    /// <summary>
    /// Gets or sets the component.
    /// </summary>
    /// <value>
    /// The component.
    /// </value>
    IComponent Component { get; set; }

    /// <summary>
    /// Gets or sets the quad.
    /// </summary>
    /// <value>
    /// The quad.
    /// </value>
    Quad Quad { get; set; }
}