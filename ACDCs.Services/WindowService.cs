using ACDCs.Interfaces;
using ACDCs.Interfaces.Modules;
using ACDCs.Interfaces.View;
using ACDCs.Structs;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Reflection;

namespace ACDCs.Services;

/// <summary>
/// The service for window handling.
/// </summary>
/// <seealso cref="ACDCs.Interfaces.IWindowService" />
public class WindowService(ILogger logger, IThemeService themeService) : IWindowService
{
    private readonly ILogger _logger = logger;
    private readonly ConcurrentDictionary<Guid, IAppModule> _modules = new();
    private readonly IThemeService _themeService = themeService;
    private IDesktopView? _desktopView;

    /// <summary>
    /// Occurs when [on window changed].
    /// </summary>
    public event EventHandler<IWindowChangedEventArgs>? OnWindowChanged;

    /// <summary>
    /// Gets the automatic start views.
    /// </summary>
    /// <returns></returns>
    public List<TypeInfo> GetAutoStartViews()
    {
        return AppDomain
                    .CurrentDomain
                    .GetAssemblies()
                    .SelectMany(a => a.DefinedTypes)
                    .Where(t => t.ImplementedInterfaces.Contains(typeof(IAutoStartModule)))
                    .ToList();
    }

    /// <summary>
    /// Gets the module views.
    /// </summary>
    /// <returns></returns>
    public List<TypeInfo> GetModuleViews()
    {
        return AppDomain
                    .CurrentDomain
                    .GetAssemblies()
                    .SelectMany(a => a.DefinedTypes)
                    .Where(t => t.ImplementedInterfaces.Contains(typeof(IStartMenuModule)))
                    .ToList();
    }

    /// <summary>
    /// Popups the specified app module
    /// </summary>
    /// <param name="typeInfo">The type information.</param>
    public void Popup(TypeInfo typeInfo)
    {
        IAppModule? module = _modules.Values.FirstOrDefault(m => m.GetType() == typeInfo.AsType());
        if (module == null)
        {
            Start(typeInfo);
        }
        else
        {
            _desktopView?.BringToFront(module);
        }
    }

    /// <summary>
    /// Sets the desktop.
    /// </summary>
    /// <param name="desktopView">The desktop view.</param>
    public void SetDesktop(IDesktopView desktopView)
    {
        _desktopView = desktopView;
    }

    /// <summary>
    /// Starts the specified module view.
    /// </summary>
    /// <param name="moduleView">The module view.</param>
    public void Start(TypeInfo moduleView)
    {
        if (Activator.CreateInstance(moduleView.AsType(), [_logger, _themeService, _desktopView]) is IAppModule appModule)
        {
            IAppModule module = appModule;
            _modules.TryAdd(module.ModuleGuid, module);
            _desktopView?.StartModule(module);
            OnWindowChanged?.Invoke(this, new WindowChangedEventArgs(module, [.. _modules.Values]));
        }
    }

    /// <summary>
    /// Stops the specified module.
    /// </summary>
    /// <param name="module">The module.</param>
    public void Stop(IAppModule module)
    {
        module.Content.Clear();
        _desktopView?.StopModule(module);
        _modules.TryRemove(module.ModuleGuid, out _);
        GC.Collect(GC.MaxGeneration);
        OnWindowChanged?.Invoke(this, new WindowChangedEventArgs(module, [.. _modules.Values]));
    }
}