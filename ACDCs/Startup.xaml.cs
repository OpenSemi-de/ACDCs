using ACDCs.App;
using ACDCs.Interfaces;
using ACDCs.Interfaces.View;
using MetroLog;

namespace ACDCs;

/// <summary>
/// Startup class for the app. Replaces App.xaml
/// </summary>
/// <seealso cref="Microsoft.Maui.Controls.Application" />
public partial class Startup : Application
{
    private static readonly ILogger Log = LoggerFactory.GetLogger(nameof(Startup));

    /// <summary>
    /// Initializes a new instance of the <see cref="Startup" /> class.
    /// </summary>
    /// <remarks>
    /// To be added.
    /// </remarks>
    public Startup()
    {
        InitializeComponent();

        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

        IDesktopView? desktopView = ServiceHelper.GetService<IDesktopView>();
        if (desktopView == null)
        {
            return;
        }

        MainPage = new MainPage(desktopView);
        desktopView.StartDesktop();
    }

    private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        Log.Error("Unhandled exception", e);
    }
}