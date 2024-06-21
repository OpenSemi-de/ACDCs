namespace ACDCs.Interfaces.View;

/// <summary>
/// Interace for the start menu view class.
/// </summary>
public interface IStartMenuView
{
    /// <summary>
    /// Gets a value indicating whether this instance is visible.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is visible; otherwise, <c>false</c>.
    /// </value>
    bool IsVisible { get; }

    /// <summary>
    /// Hides the Start Menu.
    /// </summary>
    void Hide();

    /// <summary>
    /// Shows the Start Menu.
    /// </summary>
    void Show();

    /// <summary>
    /// Starts this instance.
    /// </summary>
    void Start();
}