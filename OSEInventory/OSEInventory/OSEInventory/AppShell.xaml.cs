namespace OSEInventory;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        RegisterRoutes();

        BindingContext = this;
    }

    private void RegisterRoutes()
    {
        /*        Routes.Add("monkeydetails", typeof(MonkeyDetailPage));

                foreach (var item in Routes)
                {
                    Routing.RegisterRoute(item.Key, item.Value);
                }*/
    }
}