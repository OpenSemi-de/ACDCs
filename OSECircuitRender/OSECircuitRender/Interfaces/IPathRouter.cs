using OSECircuitRender.Items;

namespace OSECircuitRender.Interfaces;

public interface IPathRouter
{
    void SetItems(WorksheetItemList items, WorksheetItemList nets);

    WorksheetItemList GetTraces();
}