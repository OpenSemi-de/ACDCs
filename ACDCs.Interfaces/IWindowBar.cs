namespace ACDCs.Interfaces;

/// <summary>
/// The interface for the window bar.
/// </summary>
public interface IWindowBar
{
    /// <summary>
    /// Brings to front.
    /// </summary>
    /// <param name="moduleView">The module view.</param>
    void BringToFront(IAppModule moduleView);
}