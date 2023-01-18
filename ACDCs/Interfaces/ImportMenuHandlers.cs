using ACDCs.Components.Menu.MenuHandlers;
using ACDCs.Components.ModelSelection;
using ACDCs.Data.ACDCs.Components;
using ACDCs.IO.DB;

namespace ACDCs.Interfaces;

public class ImportMenuHandlers : MenuHandlerView
{
    public ImportMenuHandlers()
    {
        MenuHandler.Add("opendb", OpenDB);
        MenuHandler.Add("savetodb", SaveToDB);
        MenuHandler.Add("importspicemodels", ImportSpiceModels);
    }

    public async void ImportSpiceModels()
    {
        IDictionary<DevicePlatform, IEnumerable<string>> fileTypes =
            new Dictionary<DevicePlatform, IEnumerable<string>>();
        fileTypes.Add(DevicePlatform.WinUI, new List<string> { ".asc", ".lib", ".txt", ".bjt", ".dio" });
        fileTypes.Add(DevicePlatform.Android, new List<string> { "application/text", "*/*" });
        PickOptions options = new()
        {
            FileTypes = new FilePickerFileType(fileTypes),
            PickerTitle = "Open spice model file"
        };

        FileResult? result = await FilePicker.Default.PickAsync(options);
        if (result != null)
        {
            string fileName = result.FullPath;
            ComponentsView.ImportSpiceModels(fileName);
        }
    }

    public async void OpenDB()
    {
        await API.Call(() =>
        {
            DefaultModelRepository repo = new DefaultModelRepository();
            var defaultComponents = repo.GetModels();
            ComponentsView.LoadFromSource(defaultComponents);
            return Task.CompletedTask;
        });
    }

    public void SaveToDB()
    {
        DBConnection db = new("default");
        List<IElectronicComponent> newComponents = new();

        List<IElectronicComponent> components = Enumerable.Select<ComponentViewModel, IElectronicComponent>(ComponentsView.dataSource, m =>
        {
            
                return m.Model;
            

        }).ToList();
        List<IElectronicComponent> existingComponents = db.Read<IElectronicComponent>("Components");

        foreach (IElectronicComponent newComponent in components)
        {
            bool found = existingComponents.Any(existingComponent =>
                newComponent.Name == existingComponent.Name &&
                newComponent.IsFlatEqual(existingComponent));

            if (!found)
            {
                newComponents.Add(newComponent);
            }
        }

        db.Write(newComponents, "Components");
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
