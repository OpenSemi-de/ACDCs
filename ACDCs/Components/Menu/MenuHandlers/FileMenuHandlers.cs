using ACDCs.Services;

namespace ACDCs.Components.Menu.MenuHandlers;

public class FileMenuHandlers : MenuHandler
{
    public FileMenuHandlers()
    {
        MenuService.Add("new", NewFile);
        MenuService.Add("openfile", OpenFile);
        MenuService.Add("savefile", SaveFile);
        MenuService.Add("saveasfile", SaveFileAs);
    }

    private async void NewFile(object? o)
    {
        await FileService.NewFile(CircuitView);
    }

    private async void OpenFile(object? o)
    {
        await FileService.OpenFile(CircuitView);
    }

    private async void SaveFile(object? o)
    {
        await FileService.SaveFile(CircuitView, PopupPage);
    }

    private async void SaveFileAs(object? o)
    {
        await FileService.SaveFileAs(PopupPage, CircuitView);
    }
}
