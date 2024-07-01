namespace ACDCs.Interfaces.Renderer;

using ACDCs.Interfaces.Circuit;

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
    public IRenderManager RenderManager { get; }

    /// <summary>
    /// Loads the scene from json.
    /// </summary>
    /// <param name="jsonScene">The json scene.</param>
    public void LoadJson(string jsonScene);

    /// <summary>
    /// Sets the scene.
    /// </summary>
    /// <param name="scene">The scene.</param>
    void SetScene(IScene scene);
}