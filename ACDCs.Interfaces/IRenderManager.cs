using ACDCs.Interfaces.Circuit;

namespace ACDCs.Interfaces;

/// <summary>
/// The interface for the core renderer.
/// </summary>
public interface IRenderManager
{
    /// <summary>
    /// Occurs when [on invalidate].
    /// </summary>
    public event EventHandler OnInvalidate;

    /// <summary>
    /// Gets or sets a value indicating whether this instance is debug.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is debug; otherwise, <c>false</c>.
    /// </value>
    public bool IsDebug { get; }

    /// <summary>
    /// Gets the position.
    /// </summary>
    /// <value>
    /// The position.
    /// </value>
    Point Position { get; }

    /// <summary>
    /// Gets or sets the size of the step.
    /// </summary>
    /// <value>
    /// The size of the step.
    /// </value>
    public float StepSize { get; set; }

    /// <summary>
    /// Adds the component.
    /// </summary>
    /// <param name="component">The component.</param>
    void AddComponent(IComponent component);

    /// <summary>
    /// Gets the tapped.
    /// </summary>
    /// <param name="clickPoint">The click point.</param>
    void GetTapped(Point? clickPoint);

    /// <summary>
    /// Loads the scene from json.
    /// </summary>
    /// <param name="jsonScene">The json scene.</param>
    public void LoadJson(string jsonScene);

    /// <summary>
    /// Sets the position offset.
    /// </summary>
    /// <param name="x">The x.</param>
    /// <param name="y">The y.</param>
    public void SetPositionOffset(float x, float y);

    /// <summary>
    /// Sets the scene.
    /// </summary>
    /// <param name="scene">The scene.</param>
    void SetScene(IScene scene);
}