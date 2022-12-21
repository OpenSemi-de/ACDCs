using System;
using System.Collections.Generic;
using System.Linq;
using ACDCs.CircuitRenderer.Definitions;

namespace ACDCs.CircuitRenderer.Sheet
{
    public class Turtle
    {
        private readonly Coordinate _endCoordinate;
        private readonly Coordinate _pinAbsoluteCoordinateFrom;
        private readonly Coordinate _startCoordinate;
        private Coordinate _currentCoordinate;
        private int _stepCount;
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

        public void Run()
        {
            PathCoordinates.Add(_startCoordinate);
            Coordinate lastPosition = _pinAbsoluteCoordinateFrom;
            while (!Stuck() && !Arrived())
            {
                Coordinate nextStepOffset = GetNextStep();
                Coordinate nextStep = _currentCoordinate.Add(nextStepOffset);

                if (nextStep.IsEqual(lastPosition) )
                {
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

        private Coordinate GetNextStep()
        {
            var diffCoordinate = _currentCoordinate.Substract(_endCoordinate);
            var stepCoordinate = new Coordinate(diffCoordinate);

            if (Math.Abs(diffCoordinate.X) > Math.Abs(diffCoordinate.Y))
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
