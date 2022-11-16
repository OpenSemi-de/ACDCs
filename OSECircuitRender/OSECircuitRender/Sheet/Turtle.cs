#nullable enable

using OSECircuitRender.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Platform;
using OSECircuitRender.Drawables;
using OSECircuitRender.Interfaces;
using OSECircuitRender.Items;
using OSECircuitRender.Scene;
using Color = Microsoft.Maui.Graphics.Color;
using System.Net.NetworkInformation;
using Microsoft.VisualBasic;
using static Microsoft.Maui.Controls.Internals.GIFBitmap;

namespace OSECircuitRender.Sheet;

public enum Direction
{
    Top = 0,
    Right = 1,
    Bottom = 2,
    Left = 3,
    Contains = 4,
    None = 999,
}

public class Turtle
{
    private static readonly Point[][] DirectionalTriangles = new[]
    {
        new Point[]
        {
            new(0,0),
            new(1,0),
            new(0.5f,0.5f),
        },
        new Point[]
        {
            new(1,0),
            new(1,1),
            new(0.5f,0.5f),
        },
        new Point[]
        {
            new(1,1),
            new(0,1),
            new(0.5f,0.5f),
        },
        new Point[]
        {
            new(0,1),
            new(0,0),
            new(0.5f,0.5f),
        },
    };

    private static readonly Point[][] DirectionalTrianglesMax = new[]
    {
        new Point[]
        {
            new(0,0),
            new(int.MaxValue,0),
            new(int.MaxValue /2,int.MaxValue /2),
        },
        new Point[]
        {
            new(int.MaxValue,0),
            new(int.MaxValue, int.MaxValue),

            new(int.MaxValue /2,int.MaxValue /2),
        },
        new Point[]
        {
            new(int.MaxValue,int.MaxValue),
            new(0,int.MaxValue),

            new(int.MaxValue /2,int.MaxValue /2),
        },
        new Point[]
        {
            new(0,int.MaxValue),
            new(0, 0),

            new(int.MaxValue /2,int.MaxValue /2),
        },
    };

    private static readonly Point[] DirectionPoints = {
        new Point(0,-1),
        new Point(1,0),
        new Point(0,1),
        new Point(-1,0),
    };

    private readonly List<RectFr> _collisionRectangles = new();
    private readonly WorksheetItemList _items;
    private readonly WorksheetItemList _nets;
    private readonly Coordinate _sheetSize;
    private readonly WorksheetItemList _traces;

    public Turtle(WorksheetItemList items, WorksheetItemList nets, Coordinate sheetSize, WorksheetItemList traces)
    {
        _items = items;
        _nets = nets;
        _sheetSize = sheetSize;
        _traces = traces;
    }

    public ICanvas? DebugCanvas { get; set; }

    public static Direction LineIntersectsRect(Point p1, Point p2, RectFr r)
    {
        if (LineIntersectsLine(
             p1,
             p2,
             new Point(r.X1, r.Y1),
             new Point(r.X2, r.Y1)))
            return Direction.Top;

        if (LineIntersectsLine(
            p1,
            p2,
            new Point(r.X2, r.Y1),
            new Point(r.X2, r.Y2)))
            return Direction.Right;

        if (LineIntersectsLine(
                p1,
                p2,
                new Point(r.X2, r.Y2),
                new Point(r.X1, r.Y2)))
            return Direction.Bottom;

        if (LineIntersectsLine(
                p1,
                p2,
                new Point(r.X1, r.Y2),
                new Point(r.X1, r.Y1)))
            return Direction.Left;

        //if (r.Contains(p1) && r.Contains(p2))
        //{
        //    return Direction.Contains;
        //}

        return Direction.None;
    }

    public static bool PointInTriangle(Point pt, Point v1, Point v2, Point v3)
    {
        var d1 = Sign(pt, v1, v2);
        var d2 = Sign(pt, v2, v3);
        var d3 = Sign(pt, v3, v1);

        var hasNeg = (d1 < 0) || (d2 < 0) || (d3 < 0);
        var hasPos = (d1 > 0) || (d2 > 0) || (d3 > 0);

        return !(hasNeg && hasPos);
    }

    public static float Sign(Point p1, Point p2, Point p3)
    {
        return Convert.ToSingle((p1.X - p3.X) * (p2.Y - p3.Y) - (p2.X - p3.X) * (p1.Y - p3.Y));
    }

    public WorksheetItemList GetTraces()
    {
        foreach (var item in _items)
        {
            var rect = new RectFr
            {
                X1 = item.X,
                Y1 = item.Y,
                X2 = item.X + item.Width,
                Y2 = item.Y + item.Height
            };
            DebugDrawRectangle(rect);
            if (item.Rotation != 0)
            {
                var rotation = item.Rotation;

                Coordinate rotatedItemPos1 = RotateCoordinate(rect.X1, rect.Y1, rect.X1 + (rect.X2 - rect.X1) / 2,
                    rect.Y1 + (rect.Y2 - rect.Y1) / 2, rotation);

                Coordinate rotatedItemPos2 = RotateCoordinate(rect.X2, rect.Y2, rect.X1 + (rect.X2 - rect.X1) / 2,
                    rect.Y1 + (rect.Y2 - rect.Y1) / 2, rotation);

                rect.X1 = rotatedItemPos1.X;
                rect.Y1 = rotatedItemPos1.Y;
                rect.X2 = rotatedItemPos2.X;
                rect.Y2 = rotatedItemPos2.Y;
            }

            _collisionRectangles.Add(rect);
            DebugDrawRectangle(rect);
        }

        foreach (var net in _nets)
        {
            var trace = new TraceItem();
            for (var i = 0; i < net.Pins.Count; i++)
            {
                var pin1 = net.Pins[i];
                var pin2 = i < net.Pins.Count - 1 ? net.Pins[i + 1] : net.Pins[0];
                var pin1drawable = pin1.BackRef.DrawableComponent;
                var pin2drawable = pin2.BackRef.DrawableComponent;

                SetColorAndScaling(pin1drawable, pin2drawable);

                var position1X = pin1drawable.Position.X + (pin1.Position.X * pin1drawable.Size.X);
                var position1Y = pin1drawable.Position.Y + (pin1.Position.Y * pin1drawable.Size.Y);
                Direction direction1 = GetDirection(pin1);

                var position2X = pin2drawable.Position.X + (pin2.Position.X * pin2drawable.Size.X);
                var position2Y = pin2drawable.Position.Y + (pin2.Position.Y * pin2drawable.Size.Y);
                Direction direction2 = GetDirection(pin2);

                DebugDrawLine(
                    position1X,
                    position1Y,
                    Convert.ToSingle(position1X + DirectionPoints[(int)direction1].X / 2),
                    Convert.ToSingle(position1Y + DirectionPoints[(int)direction1].Y / 2)
                );

                var currentPoint = new Point(
                    Convert.ToSingle(position1X + DirectionPoints[(int)direction1].X / 2),
                    Convert.ToSingle(position1Y + DirectionPoints[(int)direction1].Y / 2));
                trace.AddPart(new Coordinate(position1X, position1Y, 0), Coordinate.FromPoint(currentPoint));

                var targetPoint = new Point(
                    Convert.ToSingle(position2X + DirectionPoints[(int)direction2].X / 2),
                    Convert.ToSingle(position2Y + DirectionPoints[(int)direction2].Y / 2));
                if (i != net.Pins.Count - 1)
                {
                    Direction nextDirection = GetDirectionMax(currentPoint, targetPoint);
                    Direction lastDirection;
                    bool found = false;
                    for (var f = 0; f < 10000 && !found; f++)
                    {
                        if (currentPoint == targetPoint)
                            found = true;

                        lastDirection = nextDirection;
                        nextDirection = GetDirectionMax(currentPoint, targetPoint, nextDirection);

                        var stepPoint = new Point(
                            Convert.ToSingle(currentPoint.X + DirectionPoints[(int)lastDirection].X / 2),
                            Convert.ToSingle(currentPoint.Y + DirectionPoints[(int)lastDirection].Y / 2));

                        bool tryLastDirection = false;
                        if ((stepPoint.X != targetPoint.X && stepPoint.Y != targetPoint.Y) &&
                            lastDirection != nextDirection && lastDirection != Direction.Bottom && lastDirection != Direction.Top)
                        {
                            if (currentPoint.X != targetPoint.X && currentPoint.Y != targetPoint.Y)
                            {
                                var collisionRect = _collisionRectangles
                                    .FirstOrDefault(cr =>
                                        LineIntersectsRect(currentPoint, stepPoint, cr) != Direction.None);
                                if (collisionRect == default)
                                    tryLastDirection = true;
                            }
                        }

                        if (!tryLastDirection)
                        {
                            stepPoint = new Point(
                                Convert.ToSingle(currentPoint.X + DirectionPoints[(int)nextDirection].X / 2),
                                Convert.ToSingle(currentPoint.Y + DirectionPoints[(int)nextDirection].Y / 2));

                            stepPoint = GetNextStepPoint(currentPoint, stepPoint, targetPoint, ref nextDirection);
                        }
                        else
                        {
                            nextDirection = lastDirection;
                        }

                        DebugDrawLine(Convert.ToSingle(currentPoint.X),
                                Convert.ToSingle(currentPoint.Y),
                                Convert.ToSingle(stepPoint.X), Convert.ToSingle(stepPoint.Y));
                        trace.AddPart(Coordinate.FromPoint(currentPoint), Coordinate.FromPoint(stepPoint));

                        currentPoint = stepPoint;
                    }
                }
            }

            _traces.AddItem(trace);
        }

        return _traces;
    }

    private static bool LineIntersectsLine(Point line1Point1, Point line1Point2, Point line2Point1, Point line2Point2)
    {
        float q = Convert.ToSingle((line1Point1.Y - line2Point1.Y) * (line2Point2.X - line2Point1.X) - (line1Point1.X - line2Point1.X) * (line2Point2.Y - line2Point1.Y));
        float d = Convert.ToSingle((line1Point2.X - line1Point1.X) * (line2Point2.Y - line2Point1.Y) - (line1Point2.Y - line1Point1.Y) * (line2Point2.X - line2Point1.X));

        if (d == 0)
        {
            return false;
        }

        float r = q / d;

        q = Convert.ToSingle((line1Point1.Y - line2Point1.Y) * (line1Point2.X - line1Point1.X) - (line1Point1.X - line2Point1.X) * (line1Point2.Y - line1Point1.Y));
        float s = q / d;

        if (r < 0 || r > 1 || s < 0 || s > 1)
        {
            return false;
        }

        return true;
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
                Convert.ToSingle(
                (cosTheta * (posX - centerX) -
                    sinTheta * (posY - centerY) + centerX)),
            Y =
                Convert.ToSingle(
                (sinTheta * (posX - centerX) +
                 cosTheta * (posY - centerY) + centerY))
        };
    }

    private void DebugDrawLine(float position1X, float position1Y, float position2X, float position2Y)
    {
        DebugCanvas?.DrawLine(position1X * DrawableScene.Zoom * DrawableScene.BaseGridSize, position1Y * DrawableScene.Zoom * DrawableScene.BaseGridSize, position2X * DrawableScene.Zoom * DrawableScene.BaseGridSize, position2Y * DrawableScene.Zoom * DrawableScene.BaseGridSize);
    }

    private void DebugDrawRectangle(RectFr rect)
    {
        DebugCanvas?.DrawLine(
            rect.X1 * DrawableScene.Zoom * DrawableScene.BaseGridSize,
            rect.Y1 * DrawableScene.Zoom * DrawableScene.BaseGridSize,
            rect.X2 * DrawableScene.Zoom * DrawableScene.BaseGridSize,
            rect.Y1 * DrawableScene.Zoom * DrawableScene.BaseGridSize
        );

        DebugCanvas?.DrawLine(
            rect.X2 * DrawableScene.Zoom * DrawableScene.BaseGridSize,
            rect.Y1 * DrawableScene.Zoom * DrawableScene.BaseGridSize,
            rect.X2 * DrawableScene.Zoom * DrawableScene.BaseGridSize,
            rect.Y2 * DrawableScene.Zoom * DrawableScene.BaseGridSize
        );

        DebugCanvas?.DrawLine(
            rect.X2 * DrawableScene.Zoom * DrawableScene.BaseGridSize,
            rect.Y2 * DrawableScene.Zoom * DrawableScene.BaseGridSize,
            rect.X1 * DrawableScene.Zoom * DrawableScene.BaseGridSize,
            rect.Y2 * DrawableScene.Zoom * DrawableScene.BaseGridSize
        );

        DebugCanvas?.DrawLine(
            rect.X1 * DrawableScene.Zoom * DrawableScene.BaseGridSize,
            rect.Y2 * DrawableScene.Zoom * DrawableScene.BaseGridSize,
            rect.X1 * DrawableScene.Zoom * DrawableScene.BaseGridSize,
            rect.Y1 * DrawableScene.Zoom * DrawableScene.BaseGridSize
        );
    }

    private void DebugDrawRectangle(RectF rect)
    {
        DebugCanvas?.DrawRectangle(
            rect.X * DrawableScene.Zoom * DrawableScene.BaseGridSize,
            rect.Y * DrawableScene.Zoom * DrawableScene.BaseGridSize,
            rect.Width * DrawableScene.Zoom * DrawableScene.BaseGridSize,
            rect.Height * DrawableScene.Zoom * DrawableScene.BaseGridSize
            );
    }

    private void DebugFillRect(RectF rect, int offset = 0)
    {
        DebugCanvas?.FillRectangle(
            rect.X * DrawableScene.Zoom * DrawableScene.BaseGridSize + offset,
            rect.Y * DrawableScene.Zoom * DrawableScene.BaseGridSize + offset,
            rect.Width * DrawableScene.Zoom * DrawableScene.BaseGridSize - 2 * offset,
            rect.Height * DrawableScene.Zoom * DrawableScene.BaseGridSize - 2 * offset

            );
    }

    private Direction GetDirection(PinDrawable pin1)
    {
        Direction direction = Direction.None;

        var posX = pin1.Position.X;
        var posY = pin1.Position.Y;

        int pos = 0;
        foreach (var triangle in Turtle.DirectionalTriangles)
        {
            if (PointInTriangle(
                    new Point(posX, posY),
                    triangle[0],
                    triangle[1],
                    triangle[2]
                ))
            {
                return (Direction)pos;
            }
            pos++;
        }

        return direction;
    }

    private Direction GetDirectionMax(Point centerPoint, Point measurePoint, Direction lastDirection = Direction.None)
    {
        Direction direction = default;

        var posX = measurePoint.X - centerPoint.X;
        var posY = measurePoint.Y - centerPoint.Y;
        int pos = 0;

        var directions = new List<Direction>();
        foreach (var triangle in Turtle.DirectionalTrianglesMax)
        {
            if (PointInTriangle(
                    new Point(posX + int.MaxValue / 2, posY + int.MaxValue / 2),
                    triangle[0],
                    triangle[1],
                    triangle[2]
                ))
            {
                directions.Add((Direction)pos);
            }
            pos++;
        }

        var preferHorizontal = directions.FirstOrDefault(dir => dir == Direction.Left || dir == Direction.Right);

        if (preferHorizontal == default && directions.Any(dir => dir == lastDirection) && posX > 0.5 && posY > 0.5)
        {
            direction = lastDirection;
        }

        if (preferHorizontal != default)
        {
            direction = preferHorizontal;
        }

        if (direction == default)
        {
            direction = directions.FirstOrDefault();
        }

        return direction;
    }

    private Point GetNextStepPoint(Point currentPoint, Point stepPoint, Point targetPoint, ref Direction nextDirection)
    {
        var collisionRect = _collisionRectangles
            .FirstOrDefault(cr =>
                LineIntersectsRect(currentPoint, stepPoint, cr) != Direction.None);
        int breakTries = 0;
        while (collisionRect != default)
        {
            var collisionDirection = LineIntersectsRect(currentPoint, stepPoint, collisionRect);

            Direction newDirection = Direction.None;
            switch (collisionDirection)
            {
                case Direction.Right:
                case Direction.Left:
                    newDirection = currentPoint.Y < targetPoint.Y ? Direction.Bottom : Direction.Top;
                    break;

                case Direction.Bottom:
                case Direction.Top:
                    newDirection = currentPoint.X < targetPoint.X ? Direction.Right : Direction.Left;
                    break;

                case Direction.Contains:
                    break;

                default:
                case Direction.None:
                    break;
            }

            nextDirection = newDirection;
            stepPoint = new Point(
                Convert.ToSingle(currentPoint.X + DirectionPoints[(int)newDirection].X / 2),
                Convert.ToSingle(currentPoint.Y + DirectionPoints[(int)newDirection].Y / 2));

            collisionRect = _collisionRectangles
                .FirstOrDefault(cr =>
                    LineIntersectsRect(currentPoint, stepPoint, cr) != Direction.None);
            breakTries++;
            if (breakTries > 100)
                break;
        }

        return stepPoint;
    }

    private void SetColorAndScaling(IDrawableComponent pin1drawable, IDrawableComponent pin2drawable)
    {
        var color = Color.FromRgb(red: Convert.ToInt32(pin2drawable.Position.X * 100 % 256),
            Convert.ToInt32(pin2drawable.Position.Y * 100 % 256), Convert.ToInt32(pin1drawable.Position.X * 100 % 256));
        ((ScalingCanvas)DebugCanvas).StrokeColor = color;
        ((ScalingCanvas)DebugCanvas).StrokeSize = 3;
    }
}

public class RectFr
{
    public float X1 { get; set; }
    public float X2 { get; set; }
    public float Y1 { get; set; }
    public float Y2 { get; set; }
}