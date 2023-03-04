using ACDCs.API.Core;
using Microsoft.AppCenter.Crashes;

namespace ACDCs.Application;

using API.Instance;

public partial class App : Microsoft.Maui.Controls.Application
{
    private readonly API _api;

    public App()
    {
        InitializeComponent();
        if (Current != null)
        {
            API.UserAppTheme = Current.UserAppTheme;
            API.Resources = Current.Resources;
            API.TrackError = OnError;
        }

        bool darkMode = Convert.ToBoolean(API.GetPreference("DarkMode"));
        if (darkMode)
        {
            API.UserAppTheme = AppTheme.Dark;
            UserAppTheme = AppTheme.Dark;
        }

        _api = Workbench.GetAPIInstance();
        MainPage = _api.GetWorkbenchPage();
        AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
    }

    private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        Crashes.TrackError(e.ExceptionObject as Exception);
    }

    private void OnError(Exception arg1, IDictionary<string, string> arg2, ErrorAttachmentLog[] arg3)
    {
        Crashes.TrackError(arg1, arg2, arg3);
    }
}
