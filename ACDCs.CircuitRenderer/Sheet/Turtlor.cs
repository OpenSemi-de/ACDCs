#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using ACDCs.CircuitRenderer.Definitions;
using ACDCs.CircuitRenderer.Drawables;
using ACDCs.CircuitRenderer.Interfaces;
using ACDCs.CircuitRenderer.Items;
using AStar;
using AStar.Options;
using Point = System.Drawing.Point;

namespace ACDCs.CircuitRenderer.Sheet;

public class Turtlor
{
    private readonly Dictionary<DirectionNine, Coordinate> _directionCoordinates = new()
    {
        { DirectionNine.Up, new Coordinate(0, -1, 0) },
        { DirectionNine.Down, new Coordinate(0, 1, 0) },
        { DirectionNine.Right, new Coordinate(1, 0, 0) },
        { DirectionNine.Left, new Coordinate(-1, 0, 0) }
    };

    private readonly WorksheetItemList _items;

    private readonly WorksheetItemList _nets;

    private readonly Worksheet _worksheet;
    private Dictionary<RectFr, IWorksheetItem> _debugCollisionRects;

    public short[,] CollisionMap { get; set; }

    public Dictionary<RectFr, IWorksheetItem> DebugCollisionRects
    {
        get => _debugCollisionRects;
        set => _debugCollisionRects = value;
    }

    public Turtlor(WorksheetItemList? items, WorksheetItemList? nets, Coordinate? sheetSize, Worksheet worksheet)
    {
        _worksheet = worksheet;
        _items = items ?? new WorksheetItemList(_worksheet);
        _nets = nets ?? new WorksheetItemList(_worksheet);
    }

    public static Coordinate GetAbsolutePinPosition(PinDrawable pin)
    {
        Coordinate center = pin.ParentItem.DrawableComponent.Size.Multiply(0.5f)
            .Add(pin.ParentItem.DrawableComponent.Position);

        Coordinate rotatedPinPos = new(pin.ParentItem.DrawableComponent.Position.X,
            pin.ParentItem.DrawableComponent.Position.Y);
        rotatedPinPos = rotatedPinPos.Add(pin.Position.Multiply(pin.ParentItem.DrawableComponent.Size));
        rotatedPinPos = rotatedPinPos.RotateCoordinate(center.X, center.Y, pin.ParentItem.Rotation);

        return rotatedPinPos;
    }

    public static bool PointInRect(Microsoft.Maui.Graphics.Point p1, RectFr r)
    {
        if (PointInTriangle(p1, new Microsoft.Maui.Graphics.Point(r.X1, r.Y1),
                new Microsoft.Maui.Graphics.Point(r.X2, r.Y2), new Microsoft.Maui.Graphics.Point(r.X4, r.Y4)))
            return true;
        if (PointInTriangle(p1, new Microsoft.Maui.Graphics.Point(r.X1, r.Y1),
                new Microsoft.Maui.Graphics.Point(r.X2, r.Y2), new Microsoft.Maui.Graphics.Point(r.X3, r.Y3)))
            return true;

        return false;
    }

    public static bool PointInTriangle(
        Microsoft.Maui.Graphics.Point pt,
        Microsoft.Maui.Graphics.Point v1,
        Microsoft.Maui.Graphics.Point v2,
        Microsoft.Maui.Graphics.Point v3)
    {
        float d1 = Sign(pt, v1, v2);
        float d2 = Sign(pt, v2, v3);
        float d3 = Sign(pt, v3, v1);

        bool hasNeg = d1 < 0 || d2 < 0 || d3 < 0;
        bool hasPos = d1 > 0 || d2 > 0 || d3 > 0;

        return !(hasNeg && hasPos);
    }

    public static float Sign(
        Microsoft.Maui.Graphics.Point p1,
        Microsoft.Maui.Graphics.Point p2,
        Microsoft.Maui.Graphics.Point p3)
    {
        return Convert.ToSingle((p1.X - p3.X) * (p2.Y - p3.Y) - (p2.X - p3.X) * (p1.Y - p3.Y));
    }

    public Dictionary<RectFr, IWorksheetItem> GetCollisionRects()
    {
        Dictionary<RectFr, IWorksheetItem> collList = new();
        foreach (IWorksheetItem item in _items)
        {
            RectFr rect = new(item.X,
              item.Y,
              item.X + item.Width,
              item.Y,
              item.X + item.Width,
              item.Y + item.Height,
              item.X,
              item.Y + item.Height
            );

            if (item.Rotation != 0)
            {
                float rotation = item.Rotation;
                float centerX = rect.X1 + (rect.X3 - rect.X1) / 2;
                float centerY = rect.Y1 + (rect.Y3 - rect.Y1) / 2;

                Coordinate rotatedItemPos1 = new(rect.X1, rect.Y1);
                rotatedItemPos1 = rotatedItemPos1.RotateCoordinate(centerX, centerY, rotation);
                Coordinate rotatedItemPos2 = new(rect.X2, rect.Y2);
                rotatedItemPos2 = rotatedItemPos2.RotateCoordinate(centerX, centerY, rotation);
                Coordinate rotatedItemPos3 = new(rect.X3, rect.Y3);
                rotatedItemPos3 = rotatedItemPos3.RotateCoordinate(centerX, centerY, rotation);
                Coordinate rotatedItemPos4 = new(rect.X4, rect.Y4);
                rotatedItemPos4 = rotatedItemPos4.RotateCoordinate(centerX, centerY, rotation);

                List<Coordinate> itemPositions = new()
                {
                    rotatedItemPos1, rotatedItemPos2, rotatedItemPos3, rotatedItemPos4
                };

                rect.X1 = itemPositions[0].X;
                rect.Y1 = itemPositions[0].Y;
                rect.X2 = itemPositions[1].X;
                rect.Y2 = itemPositions[1].Y;
                rect.X3 = itemPositions[2].X;
                rect.Y3 = itemPositions[2].Y;
                rect.X4 = itemPositions[3].X;
                rect.Y4 = itemPositions[3].Y;
            }

            collList.Add(rect, item);
        }

        return collList;
    }

    public Coordinate GetStepCoordinate(Coordinate position, DirectionNine direction)
    {
        return _directionCoordinates.ContainsKey(direction) ? _directionCoordinates[direction].Add(position) : new Coordinate(-100, -100, 0);
    }

    public List<WorksheetItem> GetTraces()
    {
        Dictionary<RectFr, IWorksheetItem> collisionRects = GetCollisionRects();
        DebugCollisionRects = collisionRects;
        List<WorksheetItem> traces = new();

        short[,] tiles = new short[(int)_worksheet.SheetSize.X, (int)_worksheet.SheetSize.Y];
        int width = Convert.ToInt32(_worksheet.SheetSize.X);
        int height = Convert.ToInt32(_worksheet.SheetSize.Y);
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                tiles[y, x] = 1;
                foreach (RectFr rectFr in collisionRects.Keys)
                {
                    if (PointInRect(new Microsoft.Maui.Graphics.Point(x, y), rectFr))
                    {
                        tiles[y, x] = 0;
                    }
                }
            }
        }

        CollisionMap = tiles;

        var pathfinderOptions = new PathFinderOptions
        {
            PunishChangeDirection = true,
            UseDiagonals = false,
        };

        foreach (IWorksheetItem net in _nets)
        {
            TraceItem trace = new();
            var pins = SortDistance(net.Pins);

            PinDrawable? lastPin = null;

            foreach (PinDrawable pin in pins)
            {
                if (lastPin != null)
                {
                    trace = GetTrace(trace, lastPin, pin, pathfinderOptions, tiles);
                }

                lastPin = pin;
            }

            traces.Add(trace);
        }

        return traces;
    }

    private static List<PinDrawable> SortDistance(DrawablePinList pins)
    {
        var orderedPins = pins.OrderBy(pin => pin.Position.X).ToList();

        return orderedPins;
    }

    private TraceItem GetTrace(TraceItem trace, PinDrawable fromPin, PinDrawable toPin,
        PathFinderOptions pathfinderOptions, short[,] tiles)
    {
        Coordinate pinAbsoluteCoordinateFrom = GetAbsolutePinPosition(fromPin);
        Coordinate pinAbsoluteCoordinateTo = GetAbsolutePinPosition(toPin);
        tiles[(int)pinAbsoluteCoordinateFrom.Y, (int)pinAbsoluteCoordinateFrom.X] = 999;
        tiles[(int)pinAbsoluteCoordinateTo.Y, (int)pinAbsoluteCoordinateTo.X] = 999;
        var worldGrid = new WorldGrid(tiles);

        var pathFinder = new PathFinder(worldGrid, pathfinderOptions);

        Point[] path = pathFinder.FindPath(new Point((int)pinAbsoluteCoordinateFrom.X, (int)pinAbsoluteCoordinateFrom.Y), new Point((int)pinAbsoluteCoordinateTo.X, (int)pinAbsoluteCoordinateTo.Y));
        tiles[(int)pinAbsoluteCoordinateFrom.Y, (int)pinAbsoluteCoordinateFrom.X] = 0;
        tiles[(int)pinAbsoluteCoordinateTo.Y, (int)pinAbsoluteCoordinateTo.X] = 0;

        Coordinate loopPos = pinAbsoluteCoordinateFrom;
        foreach (Point point in path)
        {
            Coordinate pathCoordinate = new(point.X, point.Y);
            trace.AddPart(loopPos, pathCoordinate);
            loopPos = pathCoordinate;
        }

        trace.AddPart(loopPos, pinAbsoluteCoordinateTo);

        return trace;
    }
}
