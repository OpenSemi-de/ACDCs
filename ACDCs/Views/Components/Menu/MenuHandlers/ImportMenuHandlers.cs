using System.Reflection;
using ACDCs.Data.ACDCs.Components;
using ACDCs.IO.DB;

namespace ACDCs.Views.Components.Menu.MenuHandlers;

public class ImportMenuHandlers : MenuHandlerView
{
    public ImportMenuHandlers()
    {
        MenuHandler.Add("savetodb", SaveToDB);
        MenuHandler.Add("importspicemodels", ImportSpiceModels);
    }

    public async void ImportSpiceModels()
    {
        IDictionary<DevicePlatform, IEnumerable<string>> fileTypes =
            new Dictionary<DevicePlatform, IEnumerable<string>>();
        fileTypes.Add(DevicePlatform.WinUI, new List<string> { ".asc", ".lib", ".txt", ".bjt", ".dio" });
        fileTypes.Add(DevicePlatform.Android, new List<string> { "application/text" });
        PickOptions options = new()
        {
            FileTypes = new FilePickerFileType(fileTypes),
            PickerTitle = "Open spice model file"
        };

        FileResult? result = await FilePicker.Default.PickAsync(options);
        if (result != null)
        {
            string fileName = result.FullPath;
            ComponentsPage.ImportSpiceModels(fileName);
        }
    }

    public void SaveToDB()
    {
        DBConnection db = new("default");
        List<IElectronicComponent> newComponents = new();

        List<IElectronicComponent> components = ComponentsPage.dataSource.Select(m => m.Model).ToList();
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

        var comparer = new ObjectsComparer.Comparer<T>();
        bool isEqual = comparer.Compare(left, right);
        return isEqual;

        if (left == null || right == null) return false;
        var propsleft = left.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
        var propsright = right.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
        if (propsright.Length != propsleft.Length)
            return false;

        string? propnamesleft = string.Join("", propsleft.Select(prop => prop.Name));
        string? propnamesright = string.Join("", propsright.Select(prop => prop.Name));

        if (propnamesright != propnamesleft)
            return false;

        foreach (PropertyInfo propLeft in propsleft)
        {
            if (propLeft.Name.ToLower().Contains("baseresist")) continue;
            var propRight = propsright.First(prop => prop.Name == propLeft.Name);
            if (Convert.ToString(propLeft.GetValue(left)) != Convert.ToString(propRight.GetValue(right)))
                return false;
        }

        return true;
    }
}
