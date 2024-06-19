using Foundation;

namespace ACDCs;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp(FileSystem.CacheDirectory);
}
