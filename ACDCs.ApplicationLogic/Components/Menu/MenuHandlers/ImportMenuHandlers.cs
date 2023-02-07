namespace ACDCs.ApplicationLogic.Components.Menu.MenuHandlers;

public class ImportMenuHandlers : MenuHandler
{
    public ComponentsView? ComponentsView
    {
        get => GetParameter<ComponentsView>("ComponentsView");
    }

    public ImportMenuHandlers()
    {
        API.Instance.Add("opendb", OpenDB);
        API.Instance.Add("savetodb", SaveToDB);
        API.Instance.Add("importspicemodels", ImportSpiceModels);
        API.Instance.Add("savejson", SaveJson);
    }

    private async void ImportSpiceModels(object? o)
    {
        if (ComponentsView != null)
        {
            await API.Instance.ImportSpiceModels(ComponentsView);
        }
    }

    private async void OpenDB(object? o)
    {
        if (ComponentsView != null)
        {
            await API.Instance.OpenDB(ComponentsView);
        }
    }

    private async void SaveJson(object? obj)
    {
        API.Instance.SaveJson();
    }

    private void SaveToDB(object? o)
    {
        if (ComponentsView != null)
        {
            API.Instance.SaveToDB(ComponentsView);
        }
    }
}
