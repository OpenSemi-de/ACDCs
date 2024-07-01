namespace ACDCs.Interfaces.Circuit;

/// <summary>
/// Debug object struct for Scenes.
/// </summary>
public class SceneDebug
{
    /// <summary>
    /// Gets or sets a value indicating whether this instance has outline.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance has outline; otherwise, <c>false</c>.
    /// </value>
    public bool HasOutline { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether [show click boxes].
    /// </summary>
    /// <value>
    ///   <c>true</c> if [show click boxes]; otherwise, <c>false</c>.
    /// </value>
    public bool ShowClickBoxes { get; set; }
}