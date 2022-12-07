using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ACDCs.Views.Components.Menu.MenuHandlers;

public class MenuHandler
{
    private static readonly Dictionary<string, Action> _menuHandlers = new();

    public static void Add(string name, Action action)
    {
        App.Call(() =>
        {
            if (!_menuHandlers.ContainsKey(name))
            {
                _menuHandlers.Add(name, action);
            }

            return Task.CompletedTask;
        }).Wait();
    }

    public static void Call(string menuCommand)
    {
        App.Call(() =>
        {
            if (_menuHandlers.ContainsKey(menuCommand))
            {
                _menuHandlers[menuCommand].Invoke();
            }

            return Task.CompletedTask;
        }).Wait();
    }
}