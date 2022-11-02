namespace OSECircuitRender
{
    public sealed class ResistorDrawable : DrawableComponent
    {
        public ResistorDrawable(IWorksheetItem backRef) : base(typeof(ResistorDrawable))
        {
            Setup(backRef);
        }

        public ResistorDrawable(IWorksheetItem backRef, string value, float x, float y) : base(typeof(ResistorDrawable))
        {
            Setup(backRef, value, x, y);
        }

        private void Setup(IWorksheetItem backRef, string value = "N/A", float x = 0, float y = 0)
        {
            DrawablePins.Add(new PinDrawable(backRef, 0.5f, 0f));
            DrawablePins.Add(new PinDrawable(backRef, 0.5f, 1f));
            DrawInstructions.Add(new Line(0.5f, 0f, 0.5f, 0.2f));
            DrawInstructions.Add(new Box(0.2f, 0.2f, 0.6f, 0.6f));
            DrawInstructions.Add(new Line(0.5f, 0.8f, 0.5f, 1f));
            DrawInstructions.Add(new Text(value, -90f, 12f, 0.7f, 0.5f));
            SetSize(1, 3);
            SetPosition(x, y);
            SetRef(backRef);
        }
    }
}