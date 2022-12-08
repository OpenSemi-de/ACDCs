using OSECircuitRender.Items;

namespace OSECircuitRender.Interfaces;

public interface IPathRouter
{
    WorksheetItemList GetTraces();

    void SetItems(WorksheetItemList items, WorksheetItemList nets);
}