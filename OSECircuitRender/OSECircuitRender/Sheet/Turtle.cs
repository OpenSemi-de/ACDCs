#nullable enable

using OSECircuitRender.Definitions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Platform;
using OSECircuitRender.Items;
using OSECircuitRender.Scene;
using Color = Microsoft.Maui.Graphics.Color;

namespace OSECircuitRender.Sheet;

public class Turtle
{
    private readonly List<RectF> _collisionRectangles = new();
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

    public static Direction LineIntersectsRect(Point p1, Point p2, RectF r)
    {
        if (LineIntersectsLine(
             p1,
             p2,
             new Point(r.X, r.Y),
             new Point(r.X + r.Width, r.Y)))
            return Direction.Top;

        if (LineIntersectsLine(
            p1,
            p2,
            new Point(r.X + r.Width, r.Y),
            new Point(r.X + r.Width, r.Y + r.Height)))
            return Direction.Right;

        if (LineIntersectsLine(
                p1,
                p2,
                new Point(r.X + r.Width, r.Y + r.Height),
                new Point(r.X, r.Y + r.Height)))
            return Direction.Bottom;

        if (LineIntersectsLine(
                p1,
                p2,
                new Point(r.X, r.Y + r.Height),
                new Point(r.X, r.Y)))
            return Direction.Left;

        if (r.Contains(p1) && r.Contains(p2))
        {
            return Direction.Contains;
        }

        return Direction.None;
    }

    public WorksheetItemList GetTraces()
    {
        foreach (var item in _items)
        {
            var rect = new RectF
            {
                X = item.X,
                Y = item.Y,
                Width = item.Width,
                Height = item.Height
            };
            rect.X += 0.1f;
            rect.Y += 0.1f;
            rect.Width -= 0.1f;
            rect.Height -= 0.1f;
            _collisionRectangles.Add(rect);
            DebugDrawRectangle(rect);
        }

        foreach (var net in _nets)
        {
            for (var i = 0; i < net.Pins.Count - 1; i++)
            {
                var pin1 = net.Pins[i];
                var pin2 = net.Pins[i + 1];
                var pin1drawable = pin1.BackRef.DrawableComponent;
                var pin2drawable = pin2.BackRef.DrawableComponent;

                var color = Color.FromRgb(red: Convert.ToInt32(pin2drawable.Position.X * 100 % 256),
                    Convert.ToInt32(pin2drawable.Position.Y * 100 % 256), Convert.ToInt32(pin1drawable.Position.X * 100 % 256));
                ((ScalingCanvas)DebugCanvas).StrokeColor = color;
                ((ScalingCanvas)DebugCanvas).StrokeSize = 3;

                var position1X = pin1drawable.Position.X + (pin1.Position.X * pin1drawable.Size.X);
                var position1Y = pin1drawable.Position.Y + (pin1.Position.Y * pin1drawable.Size.Y);
                var position2X = pin2drawable.Position.X + (pin2.Position.X * pin2drawable.Size.X);
                var position2Y = pin2drawable.Position.Y + (pin2.Position.Y * pin2drawable.Size.Y);

                DebugDrawLine(
                    position1X,
                    position1Y,
                    position2X,
                    position1Y
                    );

                DebugDrawLine(
                    position2X,
                    position1Y,
                    position2X,
                    position2Y
                    );

                foreach (var inputRect in _collisionRectangles)
                {
                    var rect = new RectF(inputRect.Location, inputRect.Size);

                    Point p1 = new Point(position1X, position1Y);
                    Point p2 = new Point(position2X, position1Y);
                    Point p3 = new Point(position2X, position2Y);

                    var intersect1 = LineIntersectsRect(p1, p2, rect);
                    var intersect2 = LineIntersectsRect(p2, p3, rect);

                    ((ScalingCanvas)DebugCanvas).FillColor = color;
                    RectF dirRect;
                    switch (intersect1)
                    {
                        case Direction.Top:
                            dirRect = new RectF(rect.X, rect.Y, rect.Width, 0.2f);
                            DebugFillRect(dirRect, 1);
                            break;

                        case Direction.Right:

                            dirRect = new RectF(rect.X + rect.Width, rect.Y, 0.2f, rect.Height);
                            DebugFillRect(dirRect, 1);
                            break;

                        case Direction.Bottom:
                            dirRect = new RectF(rect.X, rect.Y + rect.Height, rect.Width, 0.2f);
                            DebugFillRect(dirRect, 1);
                            break;

                        case Direction.Left:
                            dirRect = new RectF(rect.X, rect.Y, 0.2f, rect.Height);
                            DebugFillRect(dirRect, 1);
                            break;

                        case Direction.Contains:
                            dirRect = new RectF(rect.X, rect.Y, rect.Width, 0.2f);
                            DebugDrawRectangle(dirRect);
                            break;

                        case Direction.None:
                            break;

                        default:
                            break;
                    }
                }
            }
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

    private void DebugDrawLine(float position1X, float position1Y, float position2X, float position2Y)
    {
        DebugCanvas?.DrawLine(position1X * DrawableScene.Zoom * DrawableScene.BaseGridSize, position1Y * DrawableScene.Zoom * DrawableScene.BaseGridSize, position2X * DrawableScene.Zoom * DrawableScene.BaseGridSize, position2Y * DrawableScene.Zoom * DrawableScene.BaseGridSize);
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
}

public enum Direction
{
    Top = 0,
    Right = 1,
    Bottom = 2,
    Left = 3,
    Contains = 4,
    None = 999,
}