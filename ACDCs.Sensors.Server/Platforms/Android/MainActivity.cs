// ReSharper disable once CheckNamespace
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global
namespace ACDCs.Sensors.Server;

using Android.App;
using Android.Content.PM;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
#pragma warning disable CS8618
    public static MainActivity ActivityCurrent { get; set; }
#pragma warning restore CS8618

    public MainActivity()
    {
        ActivityCurrent = this;
    }
}
