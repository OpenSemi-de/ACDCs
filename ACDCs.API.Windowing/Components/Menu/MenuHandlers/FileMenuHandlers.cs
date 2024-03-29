﻿namespace ACDCs.API.Windowing.Components.Menu.MenuHandlers;

using Instance;
using Interfaces;

// ReSharper disable once UnusedType.Global
public class FileMenuHandlers : MenuHandler
{
    private ICircuitView? CircuitView
    {
        get => GetParameter<ICircuitView>("CircuitView");
    }

    public FileMenuHandlers()
    {
        API.Instance.Add("new", NewFile);
        API.Instance.Add("openfile", OpenFile);
        API.Instance.Add("savefile", SaveFile);
        API.Instance.Add("saveasfile", SaveFileAs);
    }

    private async void NewFile(object? o)
    {
        if (CircuitView != null)
        {
            await API.Instance.NewFile(CircuitView);
        }
    }

    private async void OpenFile(object? o)
    {
        if (CircuitView != null)
        {
            await API.Instance.OpenFile(CircuitView);
        }
    }

    private async void SaveFile(object? o)
    {
        if (CircuitView != null)
        {
            await API.Instance.SaveFile(CircuitView, API.MainPage);
        }
    }

    private async void SaveFileAs(object? o)
    {
        if (CircuitView != null)
        {
            await API.Instance.SaveFileAs(API.MainPage, CircuitView);
        }
    }
}
