namespace ACDCs.Interfaces;

/// <summary>
/// Interface for App Modules shown in the Start Menu.
/// </summary>
public interface IAppModule
{
    /// <summary>
    /// Gets the content.
    /// </summary>
    /// <value>
    /// The content.
    /// </value>
    IList<IView> Content { get; }

    /// <summary>
    /// Gets a value indicating whether this instance has taskbar entry.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance has taskbar entry; otherwise, <c>false</c>.
    /// </value>
    public bool HasTaskbarEntry { get; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is maximized.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is maximized; otherwise, <c>false</c>.
    /// </value>
    bool IsMaximized { get; set; }

    /// <summary>
    /// Gets a value indicating whether this instance is minimized.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is minimized; otherwise, <c>false</c>.
    /// </value>
    bool IsMinimized { get; set; }

    /// <summary>
    /// Gets the module unique identifier.
    /// </summary>
    /// <value>
    /// The module unique identifier.
    /// </value>
    Guid ModuleGuid { get; }

    /// <summary>
    /// Gets the title.
    /// </summary>
    /// <value>
    /// The title.
    /// </value>
    string Title { get; }

    /// <summary>
    /// Gets the index of the z.
    /// </summary>
    /// <returns></returns>
    int GetZIndex();

    /// <summary>
    /// Hides this instance.
    /// </summary>
    /// <returns></returns>
    void Hide();

    /// <summary>
    /// Maximizes this instance.
    /// </summary>
    void Maximize();

    /// <summary>
    /// Quits this instance.
    /// </summary>
    /// <returns></returns>
    void Quit();

    /// <summary>
    /// Restores this instance.
    /// </summary>
    void Restore();

    /// <summary>
    /// zs the index.
    /// </summary>
    /// <param name="zIndex">Index of the z.</param>
    void SetZIndex(int zIndex);

    /// <summary>
    /// Shows this instance.
    /// </summary>
    /// <returns></returns>
    void Show();
}