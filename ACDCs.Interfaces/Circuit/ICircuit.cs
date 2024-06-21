using ACDCs.Interfaces.Circuit;

namespace ACDCs.Interfaces;

/// <summary>
/// The interface for the circuit data structure.
/// </summary>
public interface ICircuit
{
    /// <summary>
    /// Gets the components.
    /// </summary>
    /// <value>
    /// The components.
    /// </value>
    public List<IComponent> Components { get; }
}