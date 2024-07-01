using ACDCs.Interfaces;
using ACDCs.Interfaces.Modules;

namespace ACDCs.Services;

/// <summary>
/// Window changed event args raised when a window is changed.
/// </summary>
public class WindowChangedEventArgs : IWindowChangedEventArgs
{
    private IAppModule _module;
    private List<IAppModule> _modules;

    /// <summary>
    /// Initializes a new instance of the <see cref="WindowChangedEventArgs"/> class.
    /// </summary>
    /// <param name="module">The module.</param>
    /// <param name="appModules">The application modules.</param>
    public WindowChangedEventArgs(IAppModule module, List<IAppModule> appModules)
    {
        _module = module;
        _modules = appModules;
    }

    /// <summary>
    /// Gets or sets the changed module.
    /// </summary>
    /// <value>
    /// The changed module.
    /// </value>
    public IAppModule ChangedModule { get => _module; set => _module = value; }

    /// <summary>
    /// Gets or sets the modules.
    /// </summary>
    /// <value>
    /// The modules.
    /// </value>
    public List<IAppModule> Modules { get => _modules; set => _modules = value; }
}