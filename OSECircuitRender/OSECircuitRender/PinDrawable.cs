﻿namespace OSECircuitRender
{
    public sealed class PinDrawable : DrawableComponent, IDrawableComponent
    {
        public PinDrawableType PinType { get; set; }

        public PinDrawable(IWorksheetItem backRef, float x, float y, PinDrawableType pinType = PinDrawableType.None) : base(typeof(PinDrawable))
        {
            switch (pinType)
            {
                case PinDrawableType.None:
                    {
                        DrawInstructions.Add(new Circle(0, 0, 1, 1));

                        break;
                    }
                case PinDrawableType.Null:
                case PinDrawableType.GND:
                    {
                        DrawablePins.Add(new PinDrawable(backRef, 0.5f, 0.0f, PinDrawableType.None));

                        DrawInstructions.Add(new Line(0.5f, 0f, 0.5f, 0.2f));
                        DrawInstructions.Add(new Line(0.2f, 0.2f, 0.8f, 0.2f));
                        DrawInstructions.Add(new Line(0.3f, 0.4f, 0.7f, 0.4f));
                        DrawInstructions.Add(new Line(0.4f, 0.6f, 0.6f, 0.6f));
                        DrawInstructions.Add(new Text("GND", 0, 12, 0.5f, 1.2f));

                        break;
                    };
                default:
                    break;
            }
            PinType = pinType;
            Setup(backRef, x, y);
        }

        private void Setup(IWorksheetItem backRef, float x, float y)
        {
            SetPosition(x, y);
            SetRef(backRef);
        }
    }
}