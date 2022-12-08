using System.Collections.Generic;
using System.Linq;

namespace ACDCs.Views.Components.Menu.MenuHandlers;

public class InfoMenuHandlers : MenuHandlerView
{
    public InfoMenuHandlers()
    {
        MenuHandler.Add("about", About);
        MenuHandler.Add("licenses", Licenses);
        MenuHandler.Add("debug", Debug);
    }

    private void Debug()
    {

        DebugView.IsVisible = true;

    }

    private void Licenses()
    {
    }

    private void About()
    {
    }
}
