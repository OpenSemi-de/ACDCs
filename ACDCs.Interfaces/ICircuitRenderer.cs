namespace ACDCs.Interfaces;

/// <summary>
/// The interface for the circuit renderer.
/// </summary>
public interface ICircuitRenderer
{
    /// <summary>
    /// Gets the render core.
    /// </summary>
    /// <value>
    /// The render core.
    /// </value>
    public IRenderManager RenderCore { get; }

    /// <summary>
    /// Loads the scene from json.
    /// </summary>
    /// <param name="jsonScene">The json scene.</param>
    public void LoadJson(string jsonScene);
}