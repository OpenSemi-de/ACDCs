using System.Diagnostics;
using OSECircuitRender;
using OSECircuitRender.Sheet;
using OSEInventory.Views;

namespace OSEInventory;

public delegate void DoReset(DoResetArgs? args);

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        Instance = this;

        MainPage = new AppShell();
    }

    public static event DoReset? DoReset;

    public static Worksheet? CurrentSheet { get; set; }
    public static SheetPage CurrentSheetPage { get; set; }
    public static Workbook? CurrentWorkbook { get; set; }
    public static App? Instance { get; private set; }
    public static MenuButtonView MenuButtonView { get; set; }

    public static async Task Call(Func<Task> action)
    {
        try
        {
            OnDoReset(null);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

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

    protected static void OnDoReset(DoResetArgs? args)
    {
        DoReset?.Invoke(args);
    }
}

public class DoResetArgs
{
}
