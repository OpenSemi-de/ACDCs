namespace OSEInventory;

using Microsoft.Maui.Platform;

public partial class App : Application
{
    public static App Instance { get; private set; }

    public App()
    {
        InitializeComponent();
        Instance = this;

        MainPage = new AppShell();
    }
}