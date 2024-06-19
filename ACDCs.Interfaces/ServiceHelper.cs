using System.Reflection;

namespace ACDCs.Interfaces;

/// <summary>
/// The Service Helper for service resolution.
/// </summary>
public static class ServiceHelper
{
    /// <summary>
    /// Gets the services.
    /// </summary>
    /// <value>
    /// The services.
    /// </value>
    public static IServiceProvider? Services { get; private set; }

    /// <summary>
    /// Gets the service.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetService<T>()
    {
        if (Services == null)
        {
            throw new NotImplementedException("Services could not be found");
        }

        T? service = Services.GetService<T>();

        return service == null ? throw new NotImplementedException("Service interface or implementation not found") : service;
    }

    /// <summary>
    /// Gets the service.
    /// </summary>
    /// <param name="autostartModuleView">The autostart module view.</param>
    /// <returns></returns>
    public static object? GetService(TypeInfo autostartModuleView)
    {
        if (Services == null) return null;
        return Services.GetService(autostartModuleView);
    }

    /// <summary>
    /// Initializes the specified service provider.
    /// </summary>
    /// <param name="serviceProvider">The service provider.</param>
    public static void Initialize(IServiceProvider serviceProvider) =>
        Services = serviceProvider;
}