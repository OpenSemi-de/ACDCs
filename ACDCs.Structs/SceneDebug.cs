namespace ACDCs.Interfaces.Circuit;

/// <summary>
/// Debug object struct for Scenes.
/// </summary>
public class SceneDebug
{
    /// <summary>
    /// Gets or sets a value indicating whether [draw debug].
    /// </summary>
    /// <value>
    ///   <c>true</c> if [draw debug]; otherwise, <c>false</c>.
    /// </value>
    public bool DrawDebug { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether [show click boxes].
    /// </summary>
    /// <value>
    ///   <c>true</c> if [show click boxes]; otherwise, <c>false</c>.
    /// </value>
    public bool ShowClickBoxes { get; set; } = false;
}