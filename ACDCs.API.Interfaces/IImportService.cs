namespace ACDCs.API.Interfaces;

public interface IImportService
{
    Task ImportSpiceModels(IComponentsView componentsView);

    Task OpenDB(IComponentsView componentsView);

    void SaveJson();

    void SaveToDB(IComponentsView componentsView);
}
