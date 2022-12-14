using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;

namespace ACDCs;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        MauiAppBuilder builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            
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
