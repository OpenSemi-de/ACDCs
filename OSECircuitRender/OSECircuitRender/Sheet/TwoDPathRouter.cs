using Microsoft.Maui.Graphics.Skia;
using OSECircuitRender.Definitions;
using OSECircuitRender.Interfaces;
using OSECircuitRender.Items;
using System;

namespace OSECircuitRender.Sheet;

public class TwoDPathRouter : IPathRouter
{
    private readonly Worksheet _worksheet;
    private readonly int _sheetHeight;
    private readonly int _sheetWidth;

    public TwoDPathRouter(Worksheet worksheet, Coordinate sheetSize, float gridSize)
    {
        _worksheet = worksheet;
        _sheetWidth = Convert.ToInt32(sheetSize.X);
        _sheetHeight = Convert.ToInt32(sheetSize.Y);
        SheetSize = sheetSize;
        GridSize = gridSize;
    }

    public float GridSize { get; set; }
    public WorksheetItemList Items { get; set; }
    public WorksheetItemList Nets { get; set; }
    public Coordinate SheetSize { get; set; }
    public WorksheetItemList Traces { get; set; }
    private int[,] RouteMap { get; set; }

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
}