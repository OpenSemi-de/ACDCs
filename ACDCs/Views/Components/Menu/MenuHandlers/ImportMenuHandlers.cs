using ACDCs.IO.DB;
using SpiceSharp.Entities;

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
        List<IEntity> models = ComponentsPage.dataSource.Select(m => m.Model).ToList();
        db.Write(models);
    }
}
