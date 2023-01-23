using System.Collections.Generic;
using ACDCs.CircuitRenderer.Definitions;
using ACDCs.CircuitRenderer.Items;

namespace ACDCs.CircuitRenderer.Interfaces;

public interface IPathRouter
{
    short[,] CollisionMap { get; set; }
    List<RectFr> DebugRects { get; set; }

    WorksheetItemList GetTraces();

    void SetItems(WorksheetItemList items, WorksheetItemList nets);
}
