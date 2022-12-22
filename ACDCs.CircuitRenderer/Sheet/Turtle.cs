using System;
using System.Collections.Generic;
using ACDCs.CircuitRenderer.Definitions;
using ACDCs.CircuitRenderer.Interfaces;
using Microsoft.Maui.Graphics;

namespace ACDCs.CircuitRenderer.Sheet
{
    public class Turtle
    {
        private readonly Coordinate _endCoordinate;
        private readonly Coordinate _pinAbsoluteCoordinateFrom;
        private readonly Coordinate _startCoordinate;
        private Coordinate _currentCoordinate;
        private int _stepCount;
        public Dictionary<RectFr, IWorksheetItem> CollisionRects { get; set; }
        public List<Coordinate> PathCoordinates { get; }

        public Turtle(Coordinate startCoordinate, Coordinate endCoordinate, Coordinate pinAbsoluteCoordinateFrom)
        {
            PathCoordinates = new List<Coordinate>();
            _startCoordinate = startCoordinate;
            _currentCoordinate = startCoordinate;
            _endCoordinate = endCoordinate;
            _pinAbsoluteCoordinateFrom = pinAbsoluteCoordinateFrom;
            _stepCount = 0;
        }

        public static bool LineIntersectsLine(Point line1Point1, Point line1Point2, Point line2Point1, Point line2Point2)
        {
            float q = Convert.ToSingle((line1Point1.Y - line2Point1.Y) * (line2Point2.X - line2Point1.X) -
                                       (line1Point1.X - line2Point1.X) * (line2Point2.Y - line2Point1.Y));
            float d = Convert.ToSingle((line1Point2.X - line1Point1.X) * (line2Point2.Y - line2Point1.Y) -
                                       (line1Point2.Y - line1Point1.Y) * (line2Point2.X - line2Point1.X));

            if (d == 0)
            {
                return false;
            }

            float r = q / d;

            q = Convert.ToSingle((line1Point1.Y - line2Point1.Y) * (line1Point2.X - line1Point1.X) -
                                 (line1Point1.X - line2Point1.X) * (line1Point2.Y - line1Point1.Y));
            float s = q / d;

            if (r < 0 || r > 1 || s < 0 || s > 1)
            {
                return false;
            }

            return true;
        }

        public static Direction LineIntersectsRect(Point p1, Point p2, RectFr r)
        {
            if (LineIntersectsLine(
                    p1,
                    p2,
                    new Point(r.X1, r.Y1),
                    new Point(r.X2, r.Y2)))
                return Direction.Top;

            if (LineIntersectsLine(
                    p1,
                    p2,
                    new Point(r.X2, r.Y2),
                    new Point(r.X3, r.Y3)))
                return Direction.Right;

            if (LineIntersectsLine(
                    p1,
                    p2,
                    new Point(r.X3, r.Y3),
                    new Point(r.X4, r.Y4)))
                return Direction.Bottom;

            if (LineIntersectsLine(
                    p1,
                    p2,
                    new Point(r.X4, r.Y4),
                    new Point(r.X1, r.Y1)))
                return Direction.Left;

            int hitContains = 0;
            if (PointInTriangle(p1, new Point(r.X1, r.Y1), new Point(r.X2, r.Y2), new Point(r.X4, r.Y4)))
                hitContains++;
            if (PointInTriangle(p2, new Point(r.X1, r.Y1), new Point(r.X2, r.Y2), new Point(r.X4, r.Y4)))
                hitContains++;
            if (PointInTriangle(p1, new Point(r.X1, r.Y1), new Point(r.X2, r.Y2), new Point(r.X3, r.Y3)))
                hitContains++;
            if (PointInTriangle(p2, new Point(r.X1, r.Y1), new Point(r.X2, r.Y2), new Point(r.X3, r.Y3)))
                hitContains++;

            if (hitContains > 1)
            {
                return Direction.Contains;
            }

            return Direction.None;
        }

        public static bool PointInTriangle(Point pt, Point v1, Point v2, Point v3)
        {
            float d1 = Sign(pt, v1, v2);
            float d2 = Sign(pt, v2, v3);
            float d3 = Sign(pt, v3, v1);

            bool hasNeg = (d1 < 0) || (d2 < 0) || (d3 < 0);
            bool hasPos = (d1 > 0) || (d2 > 0) || (d3 > 0);

            return !(hasNeg && hasPos);
        }

        public static float Sign(Point p1, Point p2, Point p3)
        {
            return Convert.ToSingle((p1.X - p3.X) * (p2.Y - p3.Y) - (p2.X - p3.X) * (p1.Y - p3.Y));
        }

        public void Run()
        {
            PathCoordinates.Add(_startCoordinate);
            Coordinate lastPosition = _pinAbsoluteCoordinateFrom;
            while (!Stuck() && !Arrived())
            {
                Coordinate nextStepOffset = GetNextStep();
                Coordinate nextStep = _currentCoordinate.Add(nextStepOffset);
                Direction collisionDirection =
                    !nextStep.IsEqual(_endCoordinate) ? CheckCollision(nextStepOffset, _currentCoordinate) : Direction.None;

                if (collisionDirection != Direction.None)
                {
                    (nextStepOffset.X, nextStepOffset.Y) = (nextStepOffset.Y, nextStepOffset.X);
                    nextStep = _currentCoordinate.Add(nextStepOffset);
                }

                if (nextStep.IsEqual(lastPosition))
                {
                    nextStepOffset = nextStepOffset.Multiply(-1);
                    nextStep = _currentCoordinate.Add(nextStepOffset);
                }

                PathCoordinates.Add(nextStep);
                _currentCoordinate = nextStep;
            }
        }

        private bool Arrived()
        {
            return _startCoordinate.IsEqual(_endCoordinate);
        }

        private Direction CheckCollision(Coordinate nextStepOffset, Coordinate currentCoordinate)
        {
            foreach (var collisionRect in CollisionRects.Keys)
            {
                var collisionDirection = LineIntersectsRect(
                    currentCoordinate.ToPointF(),
                    currentCoordinate.Add(nextStepOffset.Multiply(2)).ToPointF(),
                    collisionRect
                );

                if (collisionDirection != Direction.None)
                {
                    return collisionDirection;
                }
            }

            return Direction.None;
        }

        private Coordinate GetNextStep()
        {
            var diffCoordinate = _currentCoordinate.Substract(_endCoordinate);
            var stepCoordinate = new Coordinate(diffCoordinate);

            if (Math.Abs(diffCoordinate.X) > 0)
            {
                stepCoordinate.Y = 0;
                stepCoordinate.X /= Math.Abs(stepCoordinate.X);
            }
            else
            {
                stepCoordinate.Y /= Math.Abs(stepCoordinate.Y);
                stepCoordinate.X = 0;
            }

            return stepCoordinate.Multiply(-1);
        }

        private bool Stuck()
        {
            _stepCount++;
            if (_stepCount > 1000) return true;
            return false;
        }
    }
}
