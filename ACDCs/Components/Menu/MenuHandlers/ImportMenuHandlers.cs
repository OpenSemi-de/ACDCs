using ACDCs.Data.ACDCs.Components;
using ACDCs.Data.ACDCs.Interfaces;
using ACDCs.IO.DB;
using ACDCs.Services;
using MenuHandlerView = ACDCs.Views.Menu.MenuHandlerView;

namespace ACDCs.Components.Menu.MenuHandlers;

public class ImportMenuHandlers : MenuHandlerView
{
    public ImportMenuHandlers()
    {
        MenuService.Add("opendb", OpenDB);
        MenuService.Add("savetodb", SaveToDB);
        MenuService.Add("importspicemodels", ImportSpiceModels);
    }

    public async void ImportSpiceModels(object? o)
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
        ComponentsView.ImportSpiceModels(fileName);
    }

    public async void OpenDB(object? o)
    {
        await API.Call(() =>
        {
            DefaultModelRepository repo = new();
            List<IElectronicComponent> defaultComponents = repo.GetModels();
            ComponentsView.LoadFromSource(defaultComponents);
            return Task.CompletedTask;
        });
    }

    public void SaveToDB(object? o)
    {
        List<IElectronicComponent?> components = ComponentsView.dataSource.Select(m => m.Model).ToList();
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

public static class RComparer
{
    public static bool IsFlatEqual<T>(this T? left, T? right)
    {
        if (left == null || right == null)
            return false;

        ObjectsComparer.Comparer<T> comparer = new();
        bool isEqual = comparer.Compare(left, right);
        return isEqual;
    }
}
