using ACDCs.Interfaces.Modules;
using ACDCs.Interfaces.View;
using System.Reflection;

namespace ACDCs.Interfaces;

/// <summary>
/// Interface for the Window Service
/// </summary>
public interface IWindowService
{
    /// <summary>
    /// Occurs when [on window changed].
    /// </summary>
    public event EventHandler<WindowChangedEventArgs>? OnWindowChanged;

    /// <summary>
    /// Gets the automatic start views.
    /// </summary>
    /// <returns></returns>
    List<TypeInfo> GetAutoStartViews();

    /// <summary>
    /// Gets the module views.
    /// </summary>
    /// <returns></returns>
    List<TypeInfo> GetModuleViews();

    /// <summary>
    /// Popups the specified app module
    /// </summary>
    /// <param name="typeInfo">The type information.</param>
    void Popup(TypeInfo typeInfo);

    /// <summary>
    /// Sets the desktop.
    /// </summary>
    /// <param name="desktopView">The desktop view.</param>
    void SetDesktop(IDesktopView desktopView);

    /// <summary>
    /// Starts the specified module view.
    /// </summary>
    /// <param name="moduleView">The module view.</param>
    void Start(TypeInfo moduleView);

    /// <summary>
    /// Stops the specified module.
    /// </summary>
    /// <param name="module">The module.</param>
    void Stop(IAppModule module);
}