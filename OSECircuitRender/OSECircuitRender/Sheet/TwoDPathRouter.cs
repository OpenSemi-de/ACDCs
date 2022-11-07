using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using Microsoft.Maui;
using Microsoft.Maui.Platform;
using OSECircuitRender.Definitions;
using OSECircuitRender.Drawables;
using OSECircuitRender.Interfaces;
using OSECircuitRender.Items;

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

    public WorksheetItemList GetTraces()
    {
        WorksheetItemList traces = new();

        RouteMap = new int[sheetWidth + 2, sheetHeight + 2];

        foreach (var item in Items)
        {
            int itemX = item.X;
            int itemY = item.Y;
            for (var x = itemX; x < itemX + item.Width; x++)
            {
                for (var y = itemY; y < itemY + item.Height; y++)
                {
                    RouteMap[x, y] = 9;
                }
            }
        }

        foreach (var worksheetItem in Nets)
        {
            var net = (NetItem)worksheetItem;
            for (var i = 0; i < net.Pins.Count - 1; i++)
            {
                var pin = net.Pins[i];
                var nextPin = net.Pins[i + 1];
                string direction = GetDirection(pin);
                string directionNext = GetDirection(nextPin);

                int pathX = 0;
                int pathY = 0;

                switch (direction)
                {
                    case "down":
                        {
                            pathX = Convert.ToInt32(pin.BackRef.X + pin.Position.X);
                            pathY = Convert.ToInt32(pin.BackRef.Y + pin.Position.Y + pin.BackRef.Height);
                            RouteMap[pathX, pathY] = 1;
                        }
                        break;

                    case "up":
                        {
                            pathX = Convert.ToInt32(pin.BackRef.X + pin.Position.X);
                            pathY = Convert.ToInt32(pin.BackRef.Y + pin.Position.Y - 1);
                            RouteMap[pathX, pathY] = 2;
                        }
                        break;

                    case "left":
                        {
                            pathX = Convert.ToInt32(pin.Position.X - 1);
                            pathY = Convert.ToInt32(pin.Position.Y + 1);
                            RouteMap[pathX, pathY] = 3;
                        }
                        break;

                    case "right":
                        {
                            pathX = Convert.ToInt32(pin.Position.X + pin.Size.X + 1);
                            pathY = Convert.ToInt32(pin.Position.Y - 1);
                            RouteMap[pathX, pathY] = 4;
                        }
                        break;
                }
            }
        }

        string map = "";
        for (var y = 0; y < sheetHeight; y++)
        {
            for (var x = 0; x < sheetHeight; x++)
            {
                map = map + "" + RouteMap[x, y];
            }
            map += Environment.NewLine;
        }

        File.WriteAllText(Workbook.BasePath + "/lastmap.txt", map);

        Traces = traces;
        return traces;
    }

    private static string GetDirection(PinDrawable pin)
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