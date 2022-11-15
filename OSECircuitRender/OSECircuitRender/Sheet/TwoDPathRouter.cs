using Microsoft.Maui.Graphics.Skia;
using OSECircuitRender.Definitions;
using OSECircuitRender.Drawables;
using OSECircuitRender.Interfaces;
using OSECircuitRender.Items;
using System;
using System.IO;
using System.Linq;

namespace OSECircuitRender.Sheet;

public class TwoDPathRouter : IPathRouter
{
    private readonly int _sheetHeight;
    private readonly int _sheetWidth;

    public TwoDPathRouter(Coordinate sheetSize, float gridSize)
    {
        _sheetWidth = Convert.ToInt32(sheetSize.X);
        _sheetHeight = Convert.ToInt32(sheetSize.Y);
        SheetSize = sheetSize;
        GridSize = gridSize;
    }

    public float GridSize { get; set; }
    public WorksheetItemList Items { get; set; }
    public WorksheetItemList Nets { get; set; }
    public Coordinate SheetSize { get; set; }
    public WorksheetItemList Traces { get; set; } = new();
    private int[,] RouteMap { get; set; }

    public WorksheetItemList GetTraces()
    {
        var map = Workbook.DebugContext ?? new SkiaBitmapExportContext(0, 0, 10);

        var canvas = map.Canvas;

        Turtle turtle = new Turtle(Items, Nets, SheetSize, Traces);
        turtle.DebugCanvas = canvas;
        turtle.GetTraces();

        return Traces;
    }

    public void SetItems(WorksheetItemList items, WorksheetItemList nets)
    {
        Items = items;
        Nets = nets;
    }

    private static Coordinate RotateCoordinate(float posX, float posY, float centerX, float centerY,
        double angleInDegrees)
    {
        var angleInRadians = angleInDegrees * (Math.PI / 180);
        var cosTheta = Math.Cos(angleInRadians);
        var sinTheta = Math.Sin(angleInRadians);
        return new Coordinate
        {
            X =
                (int)
                (cosTheta * (posX - centerX) -
                    sinTheta * (posY - centerY) + centerX),
            Y =
                (int)
                (sinTheta * (posX - centerX) +
                 cosTheta * (posY - centerY) + centerY)
        };
    }
}