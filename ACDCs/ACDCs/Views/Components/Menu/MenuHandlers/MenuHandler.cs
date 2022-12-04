using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;

namespace ACDCs.Views.Components.Menu.MenuHandlers;

public class MenuHandler
{
    private static readonly Dictionary<string, Action> _menuHandlers = new();

    public static void Add(string name, Action action)
    {
        if(!_menuHandlers.ContainsKey(name))
            _menuHandlers.Add(name, action);
    }

    public static void Call(string menuCommand)
    {
        if (_menuHandlers.ContainsKey(menuCommand))
        {
            _menuHandlers[menuCommand].Invoke();
        }
    }
}