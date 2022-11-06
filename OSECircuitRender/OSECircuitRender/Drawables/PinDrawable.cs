using System.Text;
using Microsoft.Maui;
using OSECircuitRender.Instructions;
using OSECircuitRender.Interfaces;

namespace OSECircuitRender.Drawables
{
    public sealed class PinDrawable : DrawableComponent, IDrawableComponent
    {
        public PinDrawableType PinType { get; set; }

        public PinDrawable(IWorksheetItem backRef, float x, float y, PinDrawableType pinType = PinDrawableType.None, string pinName = "") : base(typeof(PinDrawable))
        {
            PinType = pinType;
            PinText = pinName != "" ? pinName : PinType.ToString();
            if (PinText == "Null")
                PinText = "";

            switch (pinType)
            {
                case PinDrawableType.None:
                    {
                        DrawInstructions.Add(new CircleInstruction(0, 0, 1, 1));
                        DrawInstructions.Add(new TextInstruction(PinText, 0, 12, 0.5f, 1.2f));

                        break;
                    }
                case PinDrawableType.Null:
                case PinDrawableType.Gnd:
                    {
                        DrawablePins.Add(new PinDrawable(backRef, 0.5f, 0.0f));

                        DrawInstructions.Add(new LineInstruction(0.5f, 0f, 0.5f, 0.2f));
                        DrawInstructions.Add(new LineInstruction(0.2f, 0.2f, 0.8f, 0.2f));
                        DrawInstructions.Add(new LineInstruction(0.3f, 0.4f, 0.7f, 0.4f));
                        DrawInstructions.Add(new LineInstruction(0.4f, 0.6f, 0.6f, 0.6f));
                        DrawInstructions.Add(new TextInstruction(PinText, 0, 12, 0.5f, 1.2f));

                        break;
                    }
            }
            Setup(backRef, x, y);
        }

        public string PinText { get; set; }

        private void Setup(IWorksheetItem backRef, float x, float y)
        {
            backRef.Pins.Add(this);
            SetPosition(x, y);
            SetRef(backRef);
        }
    }
}