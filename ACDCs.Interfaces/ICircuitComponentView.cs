namespace ACDCs.Interfaces;

/// <summary>
/// The interface for the circuits components (resistor, diode, ...).
/// </summary>
public interface ICircuitComponentView
{
    /// <summary>
    /// Gets or sets the render core.
    /// </summary>
    /// <value>
    /// The render core.
    /// </value>
    IRenderManager? RenderCore { get; set; }
}