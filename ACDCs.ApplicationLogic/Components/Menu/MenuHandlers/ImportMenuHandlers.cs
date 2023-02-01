namespace ACDCs.ApplicationLogic.Components.Menu.MenuHandlers;

public class ImportMenuHandlers : MenuHandler
{
    public ImportMenuHandlers()
    {
        API.Instance.Add("opendb", OpenDB);
        API.Instance.Add("savetodb", SaveToDB);
        API.Instance.Add("importspicemodels", ImportSpiceModels);
        API.Instance.Add("savejson", SaveJson);
    }

    private async void ImportSpiceModels(object? o)
    {
        await API.Instance.ImportSpiceModels(ComponentsView);
    }

    private async void OpenDB(object? o)
    {
        await API.Instance.OpenDB(ComponentsView);
    }

    private async void SaveJson(object? obj)
    {
        API.Instance.SaveJson();
    }

    private void SaveToDB(object? o)
    {
        API.Instance.SaveToDB(ComponentsView);
    }
}
