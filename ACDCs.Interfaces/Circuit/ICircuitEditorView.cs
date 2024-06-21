namespace ACDCs.Interfaces;

/// <summary>
/// The interface for the circuit editor view.
/// </summary>
public interface ICircuitEditorView
{
    /// <summary>
    /// Gets the render core.
    /// </summary>
    /// <value>
    /// The render core.
    /// </value>
    IRenderManager? RenderCore { get; }
}