using ACDCs.Views;
using Microsoft.AppCenter.Crashes;

namespace ACDCs;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        bool DarkMode = Convert.ToBoolean(API.GetPreference("DarkMode"));
        if (DarkMode)
        {
            UserAppTheme = AppTheme.Dark;
        }

        MainPage = new StartCenterPage();

        AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
    }

    private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        Crashes.TrackError(e.ExceptionObject as Exception);
    }
}
