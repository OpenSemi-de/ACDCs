namespace ACDCs.Interfaces;

/// <summary>
/// Interface for the DesktopView class.
/// </summary>
public interface IDesktopView
{
    /// <summary>
    /// Occurs when [on desktop click].
    /// </summary>
    event EventHandler? OnDesktopClick;

    /// <summary>
    /// Adds the component.
    /// </summary>
    /// <param name="view">The view.</param>
    void AddComponent(IView view);

    /// <summary>
    /// Brings to front.
    /// </summary>
    /// <param name="moduleView">The module view.</param>
    void BringToFront(IAppModule moduleView);

    /// <summary>
    /// Starts the desktop.
    /// </summary>
    /// <returns></returns>
    Task StartDesktop();

    /// <summary>
    /// Starts the module.
    /// </summary>
    /// <param name="module">The module.</param>
    void StartModule(IAppModule module);

    /// <summary>
    /// Stops the specified module view.
    /// </summary>
    /// <param name="moduleView">The module view.</param>
    void Stop(IAppModule moduleView);

    /// <summary>
    /// Stops the module.
    /// </summary>
    /// <param name="module">The module.</param>
    /// <returns></returns>
    void StopModule(IAppModule module);
}