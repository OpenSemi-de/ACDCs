using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Skia;
using OSECircuitRender.Definitions;
using OSECircuitRender.Drawables;
using OSECircuitRender.Interfaces;
using OSECircuitRender.Items;
using System;
using System.IO;

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
            float centerX = item.X + (item.Width / 2);
            float centerY = item.Y + (item.Height / 2);
            for (var x = Convert.ToInt32(item.X); x < item.X + item.Width; x++)
                for (var y = Convert.ToInt32(item.Y); y < item.Y + item.Height; y++)
                {
                    var rotatedCoordinate = RotateCoordinate(
                        x, y,
                        centerX, centerY, item.Rotation
                    );
                    RouteMap[
                        Convert.ToInt32(Math.Round(rotatedCoordinate.X)),
                        Convert.ToInt32(Math.Round(rotatedCoordinate.Y))
                    ] = 255;
                }
            foreach (var pin in item.Pins)
            {
                pinNr++;
                float pinX = item.X + (pin.Position.X * item.Width);
                float pinY = item.Y + (pin.Position.Y * item.Height);
                var rotatedCoordinate = RotateCoordinate(
                    pinX, pinY,
                    centerX, centerY, item.Rotation
                );
                RouteMap[
                    Convert.ToInt32(Math.Round(rotatedCoordinate.X)),
                    Convert.ToInt32(Math.Round(rotatedCoordinate.Y))
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

    private int[,] MapItem(IWorksheetItem item)
    {
        int[,] outputArray;
        float rotation = item.Rotation;

        int heightInt = item.Height;
        int widthInt = item.Width;
        int fieldSize = Math.Max(widthInt, heightInt) + 1;
        outputArray = new int[fieldSize, fieldSize];
        int offsetX = Convert.ToInt32(Math.Round((fieldSize - widthInt) / 2.0));
        int offsetY = Convert.ToInt32(Math.Round((fieldSize - heightInt) / 2.0));
        int ninetyDegreesSteps = Convert.ToInt32((rotation - rotation % 90) / 90);
        if (ninetyDegreesSteps > 3)
        {
            ninetyDegreesSteps %= 4;
        }

        if (ninetyDegreesSteps == 1 || ninetyDegreesSteps == 3)
        {
            for (var y = offsetY; y < heightInt; y++)
                for (var x = offsetX; x < widthInt; x++)
                    outputArray[y, x] = int.MaxValue;
        }
        else
        {
            for (var y = offsetY; y < heightInt + offsetY; y++)
                for (var x = offsetX; x < widthInt + offsetX; x++)
                    outputArray[x, y] = int.MaxValue;
        }

        double realRotation = (rotation % 90) + (ninetyDegreesSteps * 90);

        return outputArray;
    }

    private static string GetDirection(TerminalDrawable pin)
    {
        if (pin.Position.X == 0)
        {
            return "up";
        }

        if (Convert.ToInt32(Math.Round(pin.Position.X)) == 1)
        {
            return "down";
        }

        if (Convert.ToInt32(Math.Round(pin.Position.Y)) == 0)
        {
            return "left";
        }

        if (Convert.ToInt32(Math.Round(pin.Position.Y)) == 1)
        {
            return "right";
        }

        return "";
    }
}

public class TwoDPathTurtle
{
    public int X { get; }
    public int Y { get; }
    public int TargetX { get; }
    public int TargetY { get; }
    public int[,] CurrentMatrix { get; }

    public TwoDPathTurtle(int x, int y, int targetX, int targetY, int[,] currentMatrix)
    {
        X = x;
        Y = y;
        TargetX = targetX;
        TargetY = targetY;
        CurrentMatrix = currentMatrix;
    }
}