using ACDCs.CircuitRenderer.Items;

namespace ACDCs.CircuitRenderer.Interfaces;

public interface IPathRouter
{
    WorksheetItemList GetTraces();

    void SetItems(WorksheetItemList items, WorksheetItemList nets);
}
