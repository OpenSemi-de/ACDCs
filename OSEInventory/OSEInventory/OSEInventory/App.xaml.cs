using Microsoft.Maui.Controls;
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

    public static App Instance { get; private set; }
    public static Workbook CurrentWorkbook { get; set; }
    public static Worksheet CurrentSheet { get; set; }
}
