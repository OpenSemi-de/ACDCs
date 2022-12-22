using System;
using ACDCs.CircuitRenderer.Definitions;
using ACDCs.CircuitRenderer.Drawables;
using ACDCs.CircuitRenderer.Instructions;
using ACDCs.CircuitRenderer.Interfaces;
using ACDCs.CircuitRenderer.Items;
using Color = ACDCs.CircuitRenderer.Definitions.Color;

namespace ACDCs.CircuitRenderer.Sheet;

public class TwoDPathRouter : IPathRouter
{
    private readonly int _sheetHeight;

    private readonly int _sheetWidth;

    private readonly Worksheet _worksheet;

    private Color[] _traceColors = {
        new Color(255, 0, 0), new Color(0, 255, 0), new Color(0, 0, 255), new Color(255, 255, 0),
        new Color(0, 255, 255),
    };

    public float GridSize { get; set; }

    public WorksheetItemList Items { get; set; }

    public WorksheetItemList Nets { get; set; }

    public Coordinate SheetSize { get; set; }

    public WorksheetItemList Traces { get; set; }

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

    public WorksheetItemList GetTraces()
    {
        Traces.Clear();
        Turtlor turtle = new(Items, Nets, SheetSize, _worksheet);
        Traces.AddRange(turtle.GetTraces());

        int i = 0;

        foreach (IWorksheetItem worksheetItem in Traces)
        {
            if (worksheetItem is TraceItem trace)
            {
                trace.DrawableComponent.Position =
                    trace.DrawableComponent.Position.Round();
                if (trace.DrawableComponent is TraceDrawable traceDrawable)
                {
                    Color traceColor = _traceColors[i];
                    float lum = 0.5f;
                    foreach (IDrawInstruction drawInstruction in traceDrawable.DrawInstructions)
                    {
                        if (drawInstruction is LineInstruction lineInstruction)
                        {
                            lineInstruction.StrokeColor = new Color(traceColor.ToMauiColor().WithLuminosity(lum));
                            lineInstruction.Position = lineInstruction.Position.Round();
                            lineInstruction.End = lineInstruction.End.Round();
                        }

                        lum += 0.2f / traceDrawable.DrawInstructions.Count;
                    }

                    i++;
                    if (i == _traceColors.Length)
                        i = 0;
                }
            }
        }

        return Traces;
    }

    public void SetItems(WorksheetItemList items, WorksheetItemList nets)
    {
        Items.Clear();
        Items.AddRange(items);
        Nets.Clear();
        Nets.AddRange(nets);
    }
}
