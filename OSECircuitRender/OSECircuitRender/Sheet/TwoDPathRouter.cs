using System.Collections.Generic;
using OSECircuitRender.Definitions;
using OSECircuitRender.Interfaces;
using OSECircuitRender.Items;

namespace OSECircuitRender.Sheet;

public class TwoDPathRouter : IPathRouter
{
    public Coordinate SheetSize { get; set; }
    public float GridSize { get; set; }

    public WorksheetItemList Items { get; set; }

    public TwoDPathRouter(Coordinate sheetSize, float gridSize)
    {
        SheetSize = sheetSize;
        GridSize = gridSize;
    }

    public void SetItems(WorksheetItemList items)
    {
        Items = items;
    }

    public List<TraceItem> GetTraces()
    {
        List<TraceItem> traces = new();
        return traces;
    }
}