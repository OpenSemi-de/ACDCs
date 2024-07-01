using ACDCs.Interfaces.Modules;

namespace ACDCs.Interfaces.View;

/// <summary>
/// Interface for the TaskbarView class.
/// </summary>
public interface ITaskbarView
{
    /// <summary>
    /// Brings a module to front.
    /// </summary>
    /// <param name="moduleView">The module view.</param>
    void BringToFront(IAppModule moduleView);

    /// <summary>
    /// Starts the specified layout.
    /// </summary>
    /// <param name="_layout">The layout.</param>
    /// <returns></returns>
    Task Start(Sharp.UI.AbsoluteLayout _layout);
}