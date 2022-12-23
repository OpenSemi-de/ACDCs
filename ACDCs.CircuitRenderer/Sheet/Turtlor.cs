#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using ACDCs.CircuitRenderer.Definitions;
using ACDCs.CircuitRenderer.Drawables;
using ACDCs.CircuitRenderer.Interfaces;
using ACDCs.CircuitRenderer.Items;

namespace ACDCs.CircuitRenderer.Sheet;

public class Turtlor
{
    private readonly WorksheetItemList _items;

    private readonly WorksheetItemList _nets;

    private readonly Worksheet _worksheet;

    private Dictionary<DirectionNine, Coordinate> _directionCoordinates = new Dictionary<DirectionNine, Coordinate>
    {
        { DirectionNine.Up, new Coordinate(0, -1, 0) },
        { DirectionNine.Down, new Coordinate(0, 1, 0) },
        { DirectionNine.Right, new Coordinate(1, 0, 0) },
        { DirectionNine.Left, new Coordinate(-1, 0, 0) }
    };

    private Turtle _turtle;

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

    public static void Rotate(IDrawableComponent pindrawable, ref float positionX, ref float positionY, ref float pinX,
        ref float pinY)
    {
        if (pindrawable.Rotation != 0)
        {
            float centerX = pindrawable.Position.X + pindrawable.Size.X / 2;
            float centerY = pindrawable.Position.Y + pindrawable.Size.Y / 2;
            Coordinate rotatedPinPos = new(positionX, positionY);
            rotatedPinPos = rotatedPinPos.RotateCoordinate(centerX, centerY, pindrawable.Rotation);
            positionX = rotatedPinPos.X;
            positionY = rotatedPinPos.Y;
            Coordinate rotatedPinRelPos = new(pinX, pinY);
            rotatedPinRelPos.RotateCoordinate(0.5f, 0.5f, pindrawable.Rotation);
            pinX = rotatedPinRelPos.X;
            pinY = rotatedPinRelPos.Y;
        }
    }

    public Dictionary<RectFr, IWorksheetItem> GetCollisionRects()
    {
        Dictionary<RectFr, IWorksheetItem> collList = new();
        foreach (IWorksheetItem item in _items)
        {
            RectFr rect = new()
            {
                X1 = item.X,
                Y1 = item.Y,
                X2 = item.X + item.Width,
                Y2 = item.Y,
                X3 = item.X + item.Width,
                Y3 = item.Y + item.Height,
                X4 = item.X,
                Y4 = item.Y + item.Height
            };

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

    public DirectionNine GetPinStartDirection(PinDrawable pin, PinDrawable targetPin)
    {
        int pinX = Convert.ToInt32(Math.Round(pin.Position.X * 2));
        int pinY = Convert.ToInt32(Math.Round(pin.Position.Y * 2));

        DirectionNine[,] position = new DirectionNine[3, 3]
        {
            { DirectionNine.UpLeft, DirectionNine.Up, DirectionNine.UpRight },
            { DirectionNine.Left, DirectionNine.Middle, DirectionNine.Right },
            { DirectionNine.DownLeft, DirectionNine.Down, DirectionNine.DownRight }
        };

        DirectionNine direction = position[pinY, pinX];

        if (direction == DirectionNine.Middle)
        {
            direction = DirectionNine.Up;
        }
        else
        {
            if ((int)direction % 2 == 1)
            {
                direction = pin.Position.Y > targetPin.Position.Y
                    ? (DirectionNine)((int)direction + 1)
                    : (DirectionNine)((int)direction - 1);
            }
        }

        return direction;
    }

    public Coordinate GetStepCoordinate(Coordinate position, DirectionNine direction)
    {
        if (_directionCoordinates.ContainsKey(direction))
        {
            return _directionCoordinates[direction].Add(position);
        }

        return new Coordinate(-100, -100, 0);
    }

    public List<WorksheetItem> GetTraces()
    {
        GetCollisionRects();
        List<WorksheetItem> traces = new();

        foreach (IWorksheetItem net in _nets)
        {
            TraceItem trace = new();
            var pins = net.Pins
                .OrderBy(pin => pin.ParentItem.X)
                .ThenBy(pin => pin.ParentItem.Y)
                .ToList();

            PinDrawable? lastPin = null;

            foreach (PinDrawable pin in pins)
            {
                if (lastPin != null)
                {
                    trace = GetTrace(trace, lastPin, pin);
                }

                lastPin = pin;
            }

            traces.Add(trace);
        }

        return traces;
    }

    private TraceItem GetTrace(TraceItem trace, PinDrawable fromPin, PinDrawable toPin)
    {
        DirectionNine startDirectionPinFrom = GetPinStartDirection(fromPin, toPin);
        DirectionNine startDirectionPinTo = GetPinStartDirection(toPin, fromPin);

        Coordinate pinAbsoluteCoordinateFrom = GetAbsolutePinPosition(fromPin);
        Coordinate pinAbsoluteCoordinateTo = GetAbsolutePinPosition(toPin);

        Coordinate firstStepCoordinateFrom = GetStepCoordinate(pinAbsoluteCoordinateFrom, startDirectionPinFrom);
        Coordinate firstStepCoordinateTo = GetStepCoordinate(pinAbsoluteCoordinateTo, startDirectionPinTo);

        trace.AddPart(pinAbsoluteCoordinateFrom, firstStepCoordinateFrom);

        Coordinate currentPositionCoordinate = firstStepCoordinateFrom;

        _turtle = new Turtle(currentPositionCoordinate, firstStepCoordinateTo, pinAbsoluteCoordinateFrom)
        {
            CollisionRects = GetCollisionRects()
        };
        _turtle.Run();

        Coordinate loopPos = firstStepCoordinateFrom;
        foreach (Coordinate pathCoordinate in _turtle.PathCoordinates)
        {
            trace.AddPart(loopPos, pathCoordinate);
            loopPos = pathCoordinate;
        }

        trace.AddPart(pinAbsoluteCoordinateTo, firstStepCoordinateTo);

        return trace;
    }

    private int R(float number)
    {
        return Convert.ToInt32(Math.Round(number));
    }
}
