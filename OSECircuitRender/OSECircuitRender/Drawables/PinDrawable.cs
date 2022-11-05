using OSECircuitRender.Instructions;
using OSECircuitRender.Interfaces;

namespace OSECircuitRender.Drawables
{
    public sealed class PinDrawable : DrawableComponent, IDrawableComponent
    {
        public PinDrawableType PinType { get; set; }

        public PinDrawable(IWorksheetItem backRef, float x, float y, PinDrawableType pinType = PinDrawableType.None) : base(typeof(PinDrawable))
        {
            PinType = pinType;
            switch (pinType)
            {
                case PinDrawableType.None:
                    {
                        DrawInstructions.Add(new CircleInstruction(0, 0, 1, 1));

                        break;
                    }
                case PinDrawableType.Null:
                case PinDrawableType.Gnd:
                    {
                        var pinName = PinType.ToString();
                        if (pinName == "Null")
                            pinName = "";

                        DrawablePins.Add(new PinDrawable(backRef, 0.5f, 0.0f));

                        DrawInstructions.Add(new LineInstruction(0.5f, 0f, 0.5f, 0.2f));
                        DrawInstructions.Add(new LineInstruction(0.2f, 0.2f, 0.8f, 0.2f));
                        DrawInstructions.Add(new LineInstruction(0.3f, 0.4f, 0.7f, 0.4f));
                        DrawInstructions.Add(new LineInstruction(0.4f, 0.6f, 0.6f, 0.6f));
                        DrawInstructions.Add(new TextInstruction(pinName, 0, 12, 0.5f, 1.2f));

                        break;
                    }
            }
            Setup(backRef, x, y);
        }

        private void Setup(IWorksheetItem backRef, float x, float y)
        {
            SetPosition(x, y);
            SetRef(backRef);
        }
    }
}