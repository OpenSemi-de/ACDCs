namespace OSECircuitRender
{
    public sealed class ResistorItem : IWorksheetItem
    {
        public string Value { get; set; }

        public ResistorItem()
        {
            DrawableComponent = new ResistorDrawable(this);
        }

        public ResistorItem(string value, float x, float y)
        {
            DrawableComponent = new ResistorDrawable(this, value, x, y);
            Value = value;
        }

        public string RefName { get; set; }
        public IDrawableComponent DrawableComponent { get; set; }
    }
}