namespace OSEInventory;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        RegisterRoutes();
    }

    private void RegisterRoutes()
    {
        Routing.RegisterRoute(nameof(WelcomePage), typeof(WelcomePage));
        /*        Routes.Add("monkeydetails", typeof(MonkeyDetailPage));

                foreach (var item in Routes)
                {
                    Routing.RegisterRoute(item.Key, item.Value);
                }*/
    }
}
