namespace ACDCs.Interfaces;

/// <summary>
/// The interface for the view of the circuit renderer.
/// </summary>
public interface ICircuitView
{
    /// <summary>
    /// Gets the render core.
    /// </summary>
    /// <value>
    /// The render core.
    /// </value>
    IRenderManager? RenderCore { get; }

    /// <summary>
    /// Loads the scene from json.
    /// </summary>
    /// <param name="jsonScene">The json scene.</param>
    void LoadJson(string jsonScene);
}