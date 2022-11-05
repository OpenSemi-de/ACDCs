using OSECircuitRender.Instructions;
using OSECircuitRender.Interfaces;

namespace OSECircuitRender.Drawables
{
    public sealed class ResistorDrawable : DrawableComponent
    {
        public ResistorDrawable() : base(typeof(ResistorDrawable))
        {
            Setup(null);
        }

        public ResistorDrawable(IWorksheetItem backRef) : base(typeof(ResistorDrawable))
        {
            Setup(backRef);
        }

        public ResistorDrawable(IWorksheetItem backRef, string value, float x, float y) : base(typeof(ResistorDrawable))
        {
            Setup(backRef, value, x, y);
        }

        public ResistorDrawable(string value, float x, float y) : base(typeof(ResistorDrawable))
        {
            Setup(null, value, x, y);
        }

        private void Setup(IWorksheetItem backRef, string value = "N/A", float x = 0, float y = 0)
        {
            DrawablePins.Add(new PinDrawable(backRef, 0, 0.5f));
            DrawablePins.Add(new PinDrawable(backRef, 1f, 0.5f));
            DrawInstructions.Add(new LineInstruction(0f, 0.5f, 0.2f, 0.5f));
            DrawInstructions.Add(new BoxInstruction(0.2f, 0.2f, 0.6f, 0.6f));
            DrawInstructions.Add(new LineInstruction(0.8f, 0.5f, 1f, 0.5f));
            DrawInstructions.Add(new TextInstruction(value, 0f, 12f, 0.5f, 0.7f));
            SetSize(2, 1);
            SetPosition(x, y);
            SetRef(backRef);
        }
    }
}