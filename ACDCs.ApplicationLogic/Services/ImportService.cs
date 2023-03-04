namespace ACDCs.API.Core.Services;

using Components;
using Components.Components;
using Data.ACDCs.Interfaces;
using Instance;
using Interfaces;
using IO.DB;
using IO.Spice;
using DevicePlatform = Microsoft.Maui.Devices.DevicePlatform;

public class ImportService : IImportService
{
    public async Task ImportSpiceModels(IComponentsView icomponentsView)
    {
        if (icomponentsView is not ComponentsView componentsView) return;

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

    public async Task OpenDB(IComponentsView icomponentsView)
    {
        if (icomponentsView is not ComponentsView componentsView) return;

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

    public void SaveToDB(IComponentsView icomponentsView)
    {
        if (icomponentsView is not ComponentsView componentsView) return;
        List<IElectronicComponent?> components = componentsView.DataSource.Select(m => m.Model).ToList();
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
