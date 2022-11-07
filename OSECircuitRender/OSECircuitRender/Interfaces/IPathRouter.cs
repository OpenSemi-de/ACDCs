using System.Collections.Generic;
using OSECircuitRender.Items;

namespace OSECircuitRender.Interfaces;

public interface IPathRouter
{
    void SetItems(WorksheetItemList items);

    List<TraceItem> GetTraces();
}