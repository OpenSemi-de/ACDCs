using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Storage;
using CircuitView = ACDCs.Components.Circuit.CircuitView;

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
            circuitView.CurrentWorksheet.Directory = Path.GetFullPath(fileName);
            circuitView.CurrentWorksheet.Filename = Path.GetFileNameWithoutExtension(fileName);
        }
    }

    public static async Task SaveFile(CircuitView circuitView, Page popupPage)
    {
        if (circuitView.CurrentWorksheet.Filename != "")
        {
            circuitView.SaveAs(Path.Combine(circuitView.CurrentWorksheet.Directory, circuitView.CurrentWorksheet.Filename));
        }
        else
        {
            await SaveFileAs(popupPage, circuitView);
        }
    }

    public static async Task SaveFileAs(Page popupPage, CircuitView circuitView)
    {
        try
        {
            Folder filePath = await FolderPicker.Default.PickAsync(
                FileSystem.Current.AppDataDirectory,
                new CancellationToken());
            string? result = await popupPage.DisplayPromptAsync("filename", "filename",
                initialValue: Path.GetFileNameWithoutExtension(circuitView.CurrentWorksheet.Filename) + ".acc");
            if (result != null && filePath.Path != "")
            {
                circuitView.SaveAs(Path.Combine(filePath.Path, result));
            }
        }
        catch (FolderPickerException)
        {
            // i
        }
    }
}
