using ACDCs.Interfaces.Circuit;

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
    /// Adds a component.
    /// </summary>
    /// <param name="component">The component.</param>
    void AddComponent(IComponent component);

    /// <summary>
    /// Loads the scene from json.
    /// </summary>
    /// <param name="jsonScene">The json scene.</param>
    void LoadJson(string jsonScene);

    /// <summary>
    /// Sets the scene.
    /// </summary>
    /// <param name="scene">The scene.</param>
    void SetScene(IScene scene);
}