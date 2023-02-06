namespace ACDCs.ApplicationLogic.Interfaces;

using Components;

public interface IImportService
{
    Task ImportSpiceModels(ComponentsView componentsView);

    Task OpenDB(ComponentsView componentsView);

    void SaveJson();

    void SaveToDB(ComponentsView componentsView);
}
