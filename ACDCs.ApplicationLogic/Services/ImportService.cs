namespace ACDCs.ApplicationLogic.Services;

using ACDCs.Data.ACDCs.Interfaces;
using Components;
using Components.Components;
using Interfaces;
using IO.DB;
using IO.Spice;

public class ImportService : IImportService
{
    public async Task ImportSpiceModels(ComponentsView componentsView)
    {
        IDictionary<DevicePlatform, IEnumerable<string>> fileTypes =
            new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.WinUI, new List<string> { ".asc", ".lib", ".txt", ".bjt", ".dio" } },
                { DevicePlatform.Android, new List<string> { "application/text", "*/*" } }
            };

        PickOptions options = new()
        {
            FileTypes = new FilePickerFileType(fileTypes),
            PickerTitle = "Open spice model file"
        };

        FileResult? result = await FilePicker.Default.PickAsync(options);
        if (result == null)
        {
            return;
        }

        string fileName = result.FullPath;
        componentsView.ImportSpiceModels(fileName);
    }

    public async Task OpenDB(ComponentsView componentsView)
    {
        await API.Call(() =>
        {
            DefaultModelRepository repo = new();
            List<IElectronicComponent> defaultComponents = repo.GetModels();
            componentsView.LoadFromSource(defaultComponents);
            return Task.CompletedTask;
        });
    }

    public void SaveJson()
    {
        SpiceReader spiceReader = new();
        spiceReader.CreateJson();
    }

    public void SaveToDB(ComponentsView componentsView)
    {
        List<IElectronicComponent?> components = componentsView.dataSource.Select(m => m.Model).ToList();
        DefaultModelRepository repository = new();
        List<IElectronicComponent> existingComponents = repository.GetModels();

        List<IElectronicComponent?> newComponents = components
            .Select(newComponent => new
            {
                newComponent,
                found = existingComponents.Any(existingComponent =>
                    newComponent?.Name == existingComponent.Name && newComponent.IsFlatEqual(existingComponent))
            })
            .Where(t => !t.found)
            .Select(t => t.newComponent).ToList();

        repository.Write(newComponents);
    }
}
