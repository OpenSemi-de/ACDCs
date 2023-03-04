namespace ACDCs.Sensors.GUI;

using ACDCs.API.Core;
using ACDCs.API.Instance;

public partial class App : Application
{
    private readonly API _api;

    public App()
    {
        InitializeComponent();
        if (Current != null)
        {
            API.UserAppTheme = Current.UserAppTheme;
            API.Resources = Current.Resources;
            //todo     API.TrackError = OnError;
        }

        bool darkMode = Convert.ToBoolean(API.GetPreference("DarkMode"));
        //   if (darkMode)
        //      {
        API.UserAppTheme = AppTheme.Dark;
        UserAppTheme = AppTheme.Dark;
        //      }

        _api = Workbench.GetAPIInstance();
        MainPage = _api.GetWorkbenchPage();
        AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
    }

    private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
    }
}
