using ACDCs.Views.Circuit;

namespace ACDCs.Services;

public static class FileService
{
    public static async Task NewFile(CircuitView circuitView)
    {
        circuitView.Clear();
        GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true, true);
        await circuitView.Paint();
    }

    public static async Task OpenFile(CircuitView circuitView)
    {
        Dictionary<DevicePlatform, IEnumerable<string>> fileTypes =
            new()
            {
                { DevicePlatform.WinUI, new List<string> { ".acc" } },
                { DevicePlatform.Android, new List<string> { "application/acdcs" } }
            };

        PickOptions options = new()
        {
            FileTypes = new FilePickerFileType(fileTypes),
            PickerTitle = "Open circuit file"
        };

        FileResult? result = await FilePicker.Default.PickAsync(options);
        if (result != null)
        {
            string fileName = result.FullPath;
            circuitView.Open(fileName);
        }
    }

    public static async Task SaveFile(CircuitView circuitView, Page popupPage)
    {
        if (circuitView.CurrentWorksheet.Filename != "")
        {
            circuitView.SaveAs(circuitView.CurrentWorksheet.Filename);
        }
        else
        {
            await SaveFileAs(popupPage, circuitView);
        }
    }

    public static async Task SaveFileAs(Page popupPage, CircuitView circuitView)
    {
        string? result = await popupPage.DisplayPromptAsync("filename", "filename");
        if (result != null)
        {
            string mainDir = FileSystem.Current.AppDataDirectory;
            circuitView.SaveAs(Path.Combine(mainDir, result));
        }
    }
}
