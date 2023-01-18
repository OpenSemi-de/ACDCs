using ACDCs.Services;

namespace ACDCs.Components.Menu.MenuHandlers;

public class InfoMenuHandlers : Views.Menu.MenuHandlerView
{
    public InfoMenuHandlers()
    {
        MenuService.Add("about", About);
        MenuService.Add("licenses", Licenses);
        MenuService.Add("debug", Debug);
    }

    private void About(object? o)
    {
    }

    private void Debug(object? o)
    {
    }

    private void Licenses(object? o)
    {
    }
}
