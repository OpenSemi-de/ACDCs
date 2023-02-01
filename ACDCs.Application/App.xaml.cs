using ACDCs.ApplicationLogic;
using Microsoft.AppCenter.Crashes;

namespace ACDCs.Application;

public partial class App : Microsoft.Maui.Controls.Application
{
    private readonly API _api;

    public App()
    {
        InitializeComponent();
        if (App.Current != null)
        {
            API.UserAppTheme = App.Current.UserAppTheme;
            API.Resources = App.Current.Resources;
            API.TrackError = Crashes.TrackError;
        }

        bool darkMode = Convert.ToBoolean(API.GetPreference("DarkMode"));
        if (darkMode)
        {
            API.UserAppTheme = AppTheme.Dark;
            UserAppTheme = AppTheme.Dark;
        }

        _api = API.GetAPIInstance();
        MainPage = _api.GetWorkbenchPage();
        AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
    }

    private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        Crashes.TrackError(e.ExceptionObject as Exception);
    }
}
