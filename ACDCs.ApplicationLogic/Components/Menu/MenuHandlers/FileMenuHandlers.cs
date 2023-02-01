namespace ACDCs.ApplicationLogic.Components.Menu.MenuHandlers;

public class FileMenuHandlers : MenuHandler
{
    public FileMenuHandlers()
    {
        API.Instance.Add("new", NewFile);
        API.Instance.Add("openfile", OpenFile);
        API.Instance.Add("savefile", SaveFile);
        API.Instance.Add("saveasfile", SaveFileAs);
    }

    private async void NewFile(object? o)
    {
        await API.Instance.NewFile(CircuitView);
    }

    private async void OpenFile(object? o)
    {
        await API.Instance.OpenFile(CircuitView);
    }

    private async void SaveFile(object? o)
    {
        await API.Instance.SaveFile(CircuitView, PopupPage);
    }

    private async void SaveFileAs(object? o)
    {
        await API.Instance.SaveFileAs(PopupPage, CircuitView);
    }
}
