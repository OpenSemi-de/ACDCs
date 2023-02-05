namespace ACDCs.ApplicationLogic.Interfaces;

using Views;

public interface IImportService
{
    Task ImportSpiceModels(ComponentsView componentsView);

    Task OpenDB(ComponentsView componentsView);

    void SaveJson();

    void SaveToDB(ComponentsView componentsView);
}
