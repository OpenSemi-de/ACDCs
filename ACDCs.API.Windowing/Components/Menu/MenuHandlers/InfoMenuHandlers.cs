﻿namespace ACDCs.API.Windowing.Components.Menu.MenuHandlers;

using Instance;

public class InfoMenuHandlers : MenuHandler
{
    public InfoMenuHandlers()
    {
        API.Instance.Add("about", About);
        API.Instance.Add("licenses", Licenses);
        API.Instance.Add("debug", Debug);
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
