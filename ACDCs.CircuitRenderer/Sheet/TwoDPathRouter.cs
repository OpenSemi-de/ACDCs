using System;
using ACDCs.CircuitRenderer.Definitions;
using ACDCs.CircuitRenderer.Interfaces;
using ACDCs.CircuitRenderer.Items;
using Microsoft.Maui.Graphics.Skia;

namespace ACDCs.CircuitRenderer.Sheet;

public class TwoDPathRouter : IPathRouter
{
    public TwoDPathRouter(Worksheet worksheet, Coordinate sheetSize, float gridSize)
    {
        _worksheet = worksheet;
        _sheetWidth = Convert.ToInt32(sheetSize.X);
        _sheetHeight = Convert.ToInt32(sheetSize.Y);
        SheetSize = sheetSize;
        GridSize = gridSize;
        Traces = new WorksheetItemList(worksheet);
        Nets = new WorksheetItemList(worksheet);
        Items = new WorksheetItemList(worksheet);
    }

    public float GridSize { get; set; }
    public WorksheetItemList Items { get; set; }
    public WorksheetItemList Nets { get; set; }
    public Coordinate SheetSize { get; set; }
    public WorksheetItemList Traces { get; set; }

    public WorksheetItemList GetTraces()
    {
        var map = Workbook.DebugContext ?? new SkiaBitmapExportContext(0, 0, 10);

        var canvas = map.Canvas;

        Turtle turtle = new Turtle(Items, Nets, SheetSize, _worksheet)
        {
            DebugCanvas = canvas
        };
        Traces = turtle.GetTraces();

        return Traces;
    }

    public void SetItems(WorksheetItemList items, WorksheetItemList nets)
    {
        Items = items;
        Nets = nets;
    }

    private readonly int _sheetHeight;
    private readonly int _sheetWidth;
    private readonly Worksheet _worksheet;
}
