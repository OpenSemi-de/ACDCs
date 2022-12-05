using Microsoft.Maui.Devices;
using Microsoft.Maui.Storage;
using System.Collections.Generic;
using System.IO;
using Page = Microsoft.Maui.Controls.Page;

namespace ACDCs.Views.Components.Menu.MenuHandlers;

public class FileMenuHandlers: MenuHandlerView
{
    public FileMenuHandlers()
    {
        MenuHandler.Add("new", NewFile);
        MenuHandler.Add("openfile", OpenFile);
        MenuHandler.Add("savefile", SaveFile);
        MenuHandler.Add("saveasfile", SaveFileAs);

    }

    private async void NewFile()
    {
        CircuitView.Clear();
        await CircuitView.Paint();
    }

    private async void SaveFileAs()
    {


        var result = await PopupPage.DisplayPromptAsync("filename", "filename");
        if (result != null)
        {
            var fileName = result;
            string mainDir = FileSystem.Current.AppDataDirectory;
            CircuitView.SaveAs(Path.Combine(mainDir, fileName));

        }
    }

    private void SaveFile()
    {
        if (CircuitView.CurrentWorksheet.Filename != "")
        {
            CircuitView.SaveAs(CircuitView.CurrentWorksheet.Filename);
        }
        else
        {
            SaveFileAs();
        }
    }

    private async void OpenFile()
    {
        string fileName = "";
        IDictionary<DevicePlatform, IEnumerable<string>> fileTypes =
            new Dictionary<DevicePlatform, IEnumerable<string>>();
        fileTypes.Add(DevicePlatform.WinUI, new List<string>(){".acc"});
        PickOptions options = new()
        {
            FileTypes = new FilePickerFileType(fileTypes),
            PickerTitle = "Open circuit file"
        };

        var result = await FilePicker.Default.PickAsync(options);
        if (result != null)
        {
            fileName = result.FullPath;
            CircuitView.Open(fileName);
        }
    }
}