namespace ACDCs.API.Core.Services;

using ACDCs.API.Interfaces;
using CommunityToolkit.Maui.Storage;
using Components.Circuit;

public class FileService : IFileService
{
    public async Task NewFile(ICircuitView circuitView)
    {
        if (circuitView is not CircuitView circuit) return;

        circuit.Clear();
        GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true, true);
        await circuit.Paint();
    }

    public async Task OpenFile(ICircuitView circuitView)
    {
        if (circuitView is not CircuitView circuit) return;

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
            circuit.Open(fileName);
            circuit.CurrentWorksheet.Directory = Path.GetFullPath(fileName);
            circuit.CurrentWorksheet.Filename = Path.GetFileNameWithoutExtension(fileName);
        }
    }

    public async Task SaveFile(ICircuitView circuitView, Page popupPage)
    {
        if (circuitView is not CircuitView circuit) return;

        if (circuit.CurrentWorksheet.Filename != "")
        {
            circuit.SaveAs(Path.Combine(circuit.CurrentWorksheet.Directory, circuit.CurrentWorksheet.Filename));
        }
        else
        {
            await SaveFileAs(popupPage, circuitView);
        }
    }

    public async Task SaveFileAs(Page popupPage, ICircuitView circuitView)
    {
        if (circuitView is not CircuitView circuit) return;

        try
        {
            var filePath = await FolderPicker.Default.PickAsync(
                FileSystem.Current.AppDataDirectory,
                new CancellationToken());
            string? result = await popupPage.DisplayPromptAsync("filename", "filename",
                initialValue: Path.GetFileNameWithoutExtension(circuit.CurrentWorksheet.Filename) + ".acc");
            if (result != null && filePath.Folder?.Path != "")
            {
                circuit.SaveAs(Path.Combine(filePath.Folder?.Path, result));
            }
        }
        catch (FolderPickerException)
        {
            // i
        }
    }
}
