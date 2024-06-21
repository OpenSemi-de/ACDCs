namespace ACDCs.Interfaces;

/// <summary>
/// The interface for the circuit controller.
/// </summary>
public interface ICircuitControllerView
{
    /// <summary>
    /// Gets or sets the render core.
    /// </summary>
    /// <value>
    /// The render core.
    /// </value>
    IRenderManager? RenderCore { get; set; }
}