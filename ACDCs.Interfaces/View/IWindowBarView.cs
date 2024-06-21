using ACDCs.Interfaces.Modules;

namespace ACDCs.Interfaces.View;

/// <summary>
/// The interface for the window bar.
/// </summary>
public interface IWindowBarView
{
    /// <summary>
    /// Brings to front.
    /// </summary>
    /// <param name="moduleView">The module view.</param>
    void BringToFront(IAppModule moduleView);
}