using CommunityToolkit.Maui;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Crashes;

#if WINDOWS
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Windows.Graphics;
using Microsoft.Maui.LifecycleEvents;
#endif

using UraniumUI;

namespace ACDCs;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        AppCenter.Start("94d5b538-5f81-4540-82e3-e4a7e1d5a7f8", typeof(Crashes));
        MauiAppBuilder builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseUraniumUI()
            .UseUraniumUIMaterial()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("MapleMono-Regular.ttf", "MapleMonoRegular");
                fonts.AddFont("MapleMono-Bold.ttf", "MapleMonoBold");
                fonts.AddFontAwesomeIconFonts();
            });

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

        MauiApp mauiApp = builder.Build();
        return mauiApp;
    }
}
