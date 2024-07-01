using ACDCs.Interfaces.Circuit;

namespace ACDCs.Interfaces;

/// <summary>
/// Interface to the scene manager class.
/// </summary>
public interface ISceneManager
{
    /// <summary>
    /// Gets or sets the scene.
    /// </summary>
    /// <value>
    /// The scene.
    /// </value>
    IScene Scene { get; set; }

    /// <summary>
    /// Adds the component.
    /// </summary>
    /// <param name="component">The component.</param>
    void AddComponent(IComponent component);

    /// <summary>
    /// Draws on the specified canvas.
    /// </summary>
    /// <param name="canvas">The canvas.</param>
    void Draw(ICanvas canvas);

    /// <summary>
    /// Gets the tapped.
    /// </summary>
    /// <param name="clickPoint">The click point.</param>
    void GetTapped(Microsoft.Maui.Graphics.Point? clickPoint);

    /// <summary>
    /// Loads the scene from a json string.
    /// </summary>
    /// <param name="jsonScene">The json scene string.</param>
    public void LoadJson(string jsonScene);

    /// <summary>
    /// Sets the position.
    /// </summary>
    /// <param name="position">The position.</param>
    void SetPosition(Microsoft.Maui.Graphics.Point position);

    /// <summary>
    /// Sets the scene.
    /// </summary>
    /// <param name="scene">The scene.</param>
    void SetScene(IScene scene);
}