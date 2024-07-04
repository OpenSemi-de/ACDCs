using ACDCs.Interfaces.Circuit;
using ACDCs.Structs;

namespace ACDCs.Renderer;

/// <summary>
/// ClickBox class for selection handling.
/// </summary>
public class ClickBox : IClickBox
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ClickBox" /> class.
    /// </summary>
    /// <param name="component">The component.</param>
    /// <param name="quad">The quad.</param>
    public ClickBox(IComponent component, Quad quad)
    {
        Component = component;
        Quad = quad;
    }

    /// <summary>
    /// Gets or sets the component.
    /// </summary>
    /// <value>
    /// The component.
    /// </value>
    public IComponent Component { get; set; }

    /// <summary>
    /// Gets or sets the quad.
    /// </summary>
    /// <value>
    /// The quad.
    /// </value>
    public Quad Quad { get; set; }
}