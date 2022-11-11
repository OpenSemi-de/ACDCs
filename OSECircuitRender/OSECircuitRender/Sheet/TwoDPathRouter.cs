using Microsoft.Maui.Graphics;
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
    private readonly int sheetHeight;
    private readonly int sheetWidth;
    public Coordinate SheetSize { get; set; }
    public float GridSize { get; set; }

    public WorksheetItemList Items { get; set; }
    public WorksheetItemList Nets { get; set; }
    public WorksheetItemList Traces { get; set; }
    private int[,] RouteMap { get; set; }

    public TwoDPathRouter(Coordinate sheetSize, float gridSize)
    {
        sheetWidth = Convert.ToInt32(sheetSize.X);
        sheetHeight = Convert.ToInt32(sheetSize.Y);
        SheetSize = sheetSize;
        GridSize = gridSize;
    }

    public void SetItems(WorksheetItemList items, WorksheetItemList nets)
    {
        Items = items;
        Nets = nets;
    }

    private static Coordinate RotateCoordinate(float posX, float posY, float centerX, float centerY, double angleInDegrees)
    {
        double angleInRadians = angleInDegrees * (Math.PI / 180);
        double cosTheta = Math.Cos(angleInRadians);
        double sinTheta = Math.Sin(angleInRadians);
        return new Coordinate()
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

    public WorksheetItemList GetTraces()
    {
        SkiaBitmapExportContext map = Workbook.DebugContext ?? new(0, 0, 10);
        WorksheetItemList traces = new();

        RouteMap = new int[sheetWidth + 2, sheetHeight + 2];
        ICanvas canvas = map.Canvas;

        int pinNr = 0;

        foreach (var item in Items)
        {
            float lowestPinX = 0;
            float lowestPinY = 0;

            if (item.Pins.Any())
            {
                lowestPinX = item.Pins.Min(p => p.Position.X);
                lowestPinY = item.Pins.Min(p => p.Position.Y);
            }

            float centerX = item.X + (item.Width / 2) - lowestPinX;
            float centerY = item.Y + (item.Height / 2) - lowestPinY;

            for (float x = item.X; x < item.X + item.Width; x++)
                for (float y = item.Y; y < item.Y + item.Height; y++)
                {
                    var rotatedCoordinate = RotateCoordinate(
                        x, y,
                        centerX, centerY, item.Rotation
                    );
                    RouteMap[
                        Convert.ToInt32(Math.Ceiling(rotatedCoordinate.X)),
                        Convert.ToInt32(Math.Ceiling(rotatedCoordinate.Y))
                    ] = 255;
                }

            foreach (var pin in item.Pins)
            {
                pinNr++;
                float pinX = item.X + (pin.Position.X * item.Width) - lowestPinX;
                float pinY = item.Y + (pin.Position.Y * item.Height) - lowestPinY;
                var rotatedCoordinate = RotateCoordinate(
                    pinX, pinY,
                    centerX, centerY, item.Rotation
                );
                RouteMap[
                    Convert.ToInt32(Math.Ceiling(rotatedCoordinate.X)),
                    Convert.ToInt32(Math.Ceiling(rotatedCoordinate.Y))
                ] = pinNr;
            }
        }

        string mapText = "";
        for (var y = 0; y < sheetHeight; y++)
        {
            for (var x = 0; x < sheetWidth; x++)
                mapText += RouteMap[x, y] + ":";
            mapText += Environment.NewLine;
        }

        File.WriteAllText(Workbook.BasePath + "/lastmap.txt", mapText);

        foreach (var worksheetItem in Nets)
        {
        }

        Traces = traces;
        return traces;
    }
}