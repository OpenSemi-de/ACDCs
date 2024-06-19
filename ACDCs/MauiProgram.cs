using ACDCs.App.Modules;
using ACDCs.Interfaces;
using MauiIcons.Material;
using MetroLog.MicrosoftExtensions;
using MetroLog.Operators;
using Microsoft.Extensions.Logging;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

#if WINDOWS
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Windows.Graphics;
using Microsoft.Maui.LifecycleEvents;
#endif

namespace ACDCs;

/// <summary>
/// Maui base program
/// </summary>
public static partial class MauiProgram
{
    /// <summary>
    /// Creates the maui application.
    /// </summary>
    /// <param name="cacheDirectory">The cache directory.</param>
    /// <returns></returns>
    public static Microsoft.Maui.Hosting.MauiApp CreateMauiApp(string cacheDirectory)
    {
        MauiAppBuilder builder = Microsoft.Maui.Hosting.MauiApp.CreateBuilder();

#if WINDOWS
        builder.ConfigureLifecycleEvents(events =>
        {
            events.AddWindows(wndLifeCycleBuilder =>
            {
                wndLifeCycleBuilder.OnWindowCreated(window =>
                {
                    IntPtr nativeWindowHandle = WinRT.Interop.WindowNative.GetWindowHandle(window);
                    WindowId win32WindowsId = Win32Interop.GetWindowIdFromWindow(nativeWindowHandle);
                    AppWindow winuiAppWindow = AppWindow.GetFromWindowId(win32WindowsId);
                    if (winuiAppWindow.Presenter is OverlappedPresenter p)
                        p.Maximize();
                    else
                    {
                        const int width = 1200;
                        const int height = 800;
                        winuiAppWindow.MoveAndResize(new RectInt32(1920 / 2 - width / 2, 1080 / 2 - height / 2, width, height));
                    }
                });
            });
        });
#endif

#if DEBUG
        builder.Logging.AddDebug();

        builder.Logging
            .AddTraceLogger(
                options =>
                {
                    options.MinLevel = LogLevel.Trace;
                    options.MaxLevel = LogLevel.Critical;
                    options.Layout = new LogLayout();
                });
#endif

        builder.Logging
            .SetMinimumLevel(LogLevel.Trace)
            .AddInMemoryLogger(
                options =>
                {
                    options.MaxLines = 1024 * 10;
                    options.MinLevel = LogLevel.Trace;
                    options.Layout = new LogLayout();
                    options.MaxLevel = LogLevel.Critical;
                });

        builder.Logging
            .SetMinimumLevel(LogLevel.Trace)
         .AddStreamingFileLogger(
                options =>
                {
                    options.RetainDays = 2;
                    options.FolderPath = Path.Combine(
                        cacheDirectory,
                        "ACDCs.log");
                });

        builder.Services.AddSingleton(LogOperatorRetriever.Instance);

        builder
            .UseMauiApp<Startup>()
            .UseMaterialMauiIcons()
            .RegisterAppServices()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        Microsoft.Maui.Hosting.MauiApp build = builder.Build();
        GuiDllLoader.Load();
        ServiceHelper.Initialize(build.Services);

        return build;
    }
}