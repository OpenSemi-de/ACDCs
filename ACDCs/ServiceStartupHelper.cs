using ACDCs.App;
using ACDCs.App.Desktop;
using ACDCs.App.GUI.Modules;
using ACDCs.Interfaces;
using ACDCs.Interfaces.Renderer;
using ACDCs.Interfaces.View;
using ACDCs.Renderer;
using ACDCs.Renderer.Managers;
using ACDCs.Renderer.Renderers;
using ACDCs.Services;
using Microsoft.Extensions.Logging;

namespace ACDCs;

/// <summary>
/// The Service Startup Helper class.
/// </summary>
public static class ServiceStartupHelper
{
    /// <summary>
    /// Registers the application services.
    /// </summary>
    /// <param name="mauiAppBuilder">The maui application builder.</param>
    /// <returns></returns>
    public static MauiAppBuilder RegisterAppServices(this MauiAppBuilder mauiAppBuilder)
    {
        ServiceProvider serviceProvider = mauiAppBuilder.Services.BuildServiceProvider();
        ILogger<MainPage>? logger = serviceProvider.GetService<ILogger<MainPage>>();

        if (logger != null)
        {
            mauiAppBuilder.Services.AddSingleton(typeof(ILogger), logger);
        }

        mauiAppBuilder.Services.AddSingleton<IThemeService, ThemeService>((_) =>
        {
            return new ThemeService(Application.Current?.PlatformAppTheme);
        });
        mauiAppBuilder.Services.AddSingleton<IWindowService, WindowService>();
        mauiAppBuilder.Services.AddSingleton<IDesktopView, DesktopView>();
        mauiAppBuilder.Services.AddSingleton<ITaskbarView, TaskbarView>();
        mauiAppBuilder.Services.AddSingleton<IStartButtonView, StartButtonView>();
        mauiAppBuilder.Services.AddSingleton<IStartMenuView, StartMenuView>();
        mauiAppBuilder.Services.AddSingleton<IWindowBarView, WindowBarView>();

        mauiAppBuilder.Services.AddTransient<ICircuitEditorView, CircuitEditorView>();
        mauiAppBuilder.Services.AddTransient<ICircuitControllerView, CircuitControllerView>();
        mauiAppBuilder.Services.AddTransient<ICircuitComponentView, CircuitComponentView>();
        mauiAppBuilder.Services.AddTransient<ICircuitView, CircuitView>();
        mauiAppBuilder.Services.AddTransient<ICircuitRenderer, CircuitRenderer>();
        mauiAppBuilder.Services.AddTransient<IRenderManager, RenderManager>();
        mauiAppBuilder.Services.AddTransient<ISceneManager, SceneManager>();
        mauiAppBuilder.Services.AddTransient<ITextRenderer, TextRenderer>();
        mauiAppBuilder.Services.AddTransient<ILineRenderer, LineRenderer>();
        mauiAppBuilder.Services.AddTransient<IGridRenderer, GridRenderer>();
        mauiAppBuilder.Services.AddTransient<IPointRenderer, PointRenderer>();

        if (Application.Current != null)
        {
            Application.Current.RequestedThemeChanged += RequestedThemeChanged;
        }

        return mauiAppBuilder;
    }

    /// <summary>
    /// Raised if the theme changed.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="AppThemeChangedEventArgs"/> instance containing the event data.</param>
    private static void RequestedThemeChanged(object? sender, AppThemeChangedEventArgs e)
    {
        ServiceHelper.GetService<IThemeService>()?.SetTheme(e.RequestedTheme);
    }
}