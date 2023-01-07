using CommunityToolkit.Maui;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Crashes;
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
            });

        return builder.Build();
    }
}

public class AppComService
{
}
