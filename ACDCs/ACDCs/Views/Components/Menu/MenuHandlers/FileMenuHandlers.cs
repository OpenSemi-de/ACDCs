using Microsoft.Maui.Controls;

namespace ACDCs.Views.Components.Menu.MenuHandlers;

public class FileMenuHandlers: MenuHandlerView
{
    public FileMenuHandlers()
    {
        MenuHandler.Add("openfile", OpenFile);
    }

    private void OpenFile()
    {
        
    }
}