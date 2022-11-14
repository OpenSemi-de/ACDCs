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

        RouteMap = new int[_sheetWidth * 2, _sheetHeight * 2];
        var canvas = map.Canvas;

        var pinNr = 0;

        foreach (var origItem in Items)
        {
            var item = new WorksheetItem
            {
                DrawableComponent = new DrawableComponent(typeof(DrawableComponent))
                {
                    Position = new Coordinate(origItem.DrawableComponent.Position),
                    Size = new Coordinate(origItem.DrawableComponent.Size),
                    Rotation = origItem.Rotation
                }
            };
            item.DrawableComponent.Position.X *= 2;
            item.DrawableComponent.Position.Y *= 2;
            item.DrawableComponent.Size.X *= 2;
            item.DrawableComponent.Size.Y *= 2;

            item.Pins.AddRange(
                origItem.Pins.ToList().Select(
                    p =>
                        new PinDrawable(p.BackRef, p.Position.X * 2, p.Position.Y * 2)
                ));

            float lowestPinX = 0;
            float lowestPinY = 0;

            if (item.Pins.Any())
            {
                lowestPinX = item.Pins.Min(p => p.Position.X);
                lowestPinY = item.Pins.Min(p => p.Position.Y);
            }

            var centerX = item.X + item.Width / 2 - lowestPinX;
            var centerY = item.Y + item.Height / 2 - lowestPinY;

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
                var pinX = item.X + pin.Position.X * (item.Width / 2) - lowestPinX;
                var pinY = item.Y + pin.Position.Y * (item.Height / 2) - lowestPinY;
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

        foreach (var net in Nets)
        {
            var pins = net.Pins.OrderBy(p => p.Position.X).ThenBy(p => p.Position.Y)
                .Select(p => new PinDrawable(p)).ToList();

            for (var i = 0; i < pins.Count - 1; i++)
            {
                var pos1 = new Coordinate(pins[i].BackRef.DrawableComponent.Position);
                var pos2 = new Coordinate(pins[i + 1].BackRef.DrawableComponent.Position);
                var posm = new Coordinate(pins[i].BackRef.DrawableComponent.Position)
                {
                    X = pos2.X
                };
                var trace = new TraceItem();

                trace.AddPart(pos1, posm);
                trace.AddPart(posm, pos2);
                Traces.AddItem(trace);
            }

            pins.ForEach(p =>
            {
                p.Position.X *= 2;
                p.Position.Y *= 2;
                p.Size.X *= 2;
                p.Size.Y *= 2;
            });

            for (var i = 0; i < pins.Count - 1; i++)
            {
                var pin1 = pins[i];
                var pin2 = pins[i + 1];
                var tx = 1;
                var ty = 1;
                var t = new Turtle(pin1.BackRef.DrawableComponent.Position, pin2.BackRef.DrawableComponent.Position,
                    RouteMap);
                t.Crawl();

                for (var x = Convert.ToInt32(pin1.Position.X); x <= pin2.Position.X; x += 1)
                {
                    //     if (RouteMap[x, pin1.Position.Y] != 0)
                }

                for (var y = Convert.ToInt32(pin1.Position.Y); y <= pin2.Position.Y; y += 1)
                {
                }
            }
        }

        var mapText = "";
        for (var y = 0; y < _sheetHeight; y++)
        {
            for (var x = 0; x < _sheetWidth; x++)
                mapText += RouteMap[x, y] + ":";
            mapText += Environment.NewLine;
        }

        File.WriteAllText(Workbook.BasePath + "/lastmap.txt", mapText);

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

    private float DistanceX(PinDrawable pin1, PinDrawable pin2)
    {
        return pin2.Position.X - pin1.Position.X;
    }

    private float DistanceY(PinDrawable pin1, PinDrawable pin2)
    {
        return pin2.Position.Y - pin1.Position.Y;
    }
}

public static class TurtleDirection
{
    public static Coordinate Down = new(0, 1, 0);

    public static Coordinate Left = new(-1, 0, 0);

    public static Coordinate Right = new(1, 0, 0);

    public static Coordinate[] Rotation = new Coordinate[4]
    {
        Up,
        Down,
        Left,
        Right
    };

    public static Coordinate Up = new(0, -1, 0);
}