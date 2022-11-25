using System.Diagnostics;
using OSECircuitRender;
using OSECircuitRender.Sheet;

namespace OSEInventory;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        Instance = this;

        MainPage = new AppShell();
    }

    public static Worksheet? CurrentSheet { get; set; }
    public static Workbook? CurrentWorkbook { get; set; }
    public static App? Instance { get; private set; }

    public static async Task Try(Func<Task> action)
    {
        try
        {
            await action().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            //_log?.Invoke(ex.ToString());
        }
    }
}
