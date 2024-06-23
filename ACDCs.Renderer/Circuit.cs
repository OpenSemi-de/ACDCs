using ACDCs.Interfaces;
using ACDCs.Interfaces.Circuit;

namespace ACDCs.Renderer;

/// <summary>
/// The circuit data structure.
/// </summary>
/// <seealso cref="ACDCs.Interfaces.ICircuit" />
public class Circuit : ICircuit
{
    /// <summary>
    /// Gets the components.
    /// </summary>
    /// <value>
    /// The components.
    /// </value>
    public List<IComponent> Components { get; } = [];
}