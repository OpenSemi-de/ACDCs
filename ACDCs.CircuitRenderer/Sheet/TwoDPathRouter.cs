using System;
using ACDCs.CircuitRenderer.Definitions;
using ACDCs.CircuitRenderer.Drawables;
using ACDCs.CircuitRenderer.Instructions;
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

        Turtle turtle = new(Items, Nets, SheetSize, _worksheet)
        {
            DebugCanvas = canvas
        };
        Traces = turtle.GetTraces();

        int i = 0;
        Color[] traceColors = new[]
        {
            new Color(255, 0, 0), new Color(0, 255, 0), new Color(0, 0, 255), new Color(255, 255, 0),
            new Color(0, 255, 255),
        };

        foreach (var worksheetItem in Traces)
        {
            var trace = (TraceItem)worksheetItem;
            trace.DrawableComponent.Position =
                trace.DrawableComponent.Position.Round();
            if (trace.DrawableComponent is TraceDrawable traceDrawable)
            {
                Color traceColor = traceColors[i];
                foreach (var drawInstruction in traceDrawable.DrawInstructions)
                {
                    var lineInstruction = (LineInstruction)drawInstruction;
                    lineInstruction.StrokeColor = traceColor;
                    lineInstruction.Position = lineInstruction.Position.Round();
                    lineInstruction.End = lineInstruction.End.Round();
                }

                i++;
                if (i == traceColors.Length)
                    i = 0;
            }
        }

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
