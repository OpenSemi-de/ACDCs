using ACDCs.ApplicationLogic.Views;

namespace ACDCs.ApplicationLogic.Interfaces;

public interface IImportService
{
    Task ImportSpiceModels(ComponentsView componentsView);

    Task OpenDB(ComponentsView componentsView);

    void SaveJson();

    void SaveToDB(ComponentsView componentsView);
}
