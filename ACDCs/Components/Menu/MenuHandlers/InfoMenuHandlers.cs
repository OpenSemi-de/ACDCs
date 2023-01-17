namespace ACDCs.Components.Menu.MenuHandlers;

public class InfoMenuHandlers : MenuHandlerView
{
    public InfoMenuHandlers()
    {
        MenuHandler.Add("about", About);
        MenuHandler.Add("licenses", Licenses);
        MenuHandler.Add("debug", Debug);
    }

    private void About()
    {
    }

    private void Debug()
    {
    }

    private void Licenses()
    {
    }
}
