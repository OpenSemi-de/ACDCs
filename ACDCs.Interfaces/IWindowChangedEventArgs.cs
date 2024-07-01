namespace ACDCs.Interfaces;

using ACDCs.Interfaces.Modules;
using System.Reflection;

/// <summary>
/// Interface for WindowChangedEventArgs.
/// </summary>
public interface IWindowChangedEventArgs
{
    /// <summary>
    /// Gets or sets the changed module.
    /// </summary>
    /// <value>
    /// The changed module.
    /// </value>
    public IAppModule ChangedModule { get; set; }

    /// <summary>
    /// Gets or sets the modules.
    /// </summary>
    /// <value>
    /// The modules.
    /// </value>
    public List<IAppModule> Modules { get; set; }
}