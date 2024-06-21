using ACDCs.Interfaces.Modules;

namespace ACDCs.Interfaces;

/// <summary>
/// Window changed event args raised when a window is changed.
/// </summary>
public class WindowChangedEventArgs(IAppModule module, List<IAppModule> appModules)
{
    private IAppModule _module = module;
    private List<IAppModule> _modules = appModules;

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