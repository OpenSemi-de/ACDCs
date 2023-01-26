using ACDCs.Services;

namespace ACDCs.Components.Menu.MenuHandlers;

public class ImportMenuHandlers : MenuHandler
{
    public ImportMenuHandlers()
    {
        MenuService.Add("opendb", OpenDB);
        MenuService.Add("savetodb", SaveToDB);
        MenuService.Add("importspicemodels", ImportSpiceModels);
    }

    private async void ImportSpiceModels(object? o)
    {
        await ImportService.ImportSpiceModels(ComponentsView);
    }

    private async void OpenDB(object? o)
    {
        await ImportService.OpenDB(ComponentsView);
    }

    private void SaveToDB(object? o)
    {
        ImportService.SaveToDB(ComponentsView);
    }
}
