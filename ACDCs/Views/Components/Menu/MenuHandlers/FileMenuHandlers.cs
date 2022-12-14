namespace ACDCs.Views.Components.Menu.MenuHandlers;

public class FileMenuHandlers : MenuHandlerView
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
        GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true, true);
        await CircuitView.Paint();
    }

    private async void OpenFile()
    {
        IDictionary<DevicePlatform, IEnumerable<string>> fileTypes =
            new Dictionary<DevicePlatform, IEnumerable<string>>();
        fileTypes.Add(DevicePlatform.WinUI, new List<string> { ".acc" });
        fileTypes.Add(DevicePlatform.Android, new List<string> { "application/acdcs" });
        PickOptions options = new()
        {
            FileTypes = new FilePickerFileType(fileTypes),
            PickerTitle = "Open circuit file"
        };

        FileResult? result = await FilePicker.Default.PickAsync(options);
        if (result != null)
        {
            string fileName = result.FullPath;
            CircuitView.Open(fileName);
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

    private async void SaveFileAs()
    {
        string? result = await PopupPage.DisplayPromptAsync("filename", "filename");
        if (result != null)
        {
            string fileName = result;
            string mainDir = FileSystem.Current.AppDataDirectory;
            CircuitView.SaveAs(Path.Combine(mainDir, fileName));
        }
    }
}
