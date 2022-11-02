namespace OSECircuitRender
{
    public sealed class ResistorItem : IWorksheetItem
    {
        public ResistorItem()
        {
            DrawableComponent = new ResistorDrawable(this);
        }

        public ResistorItem(string value, float x, float y)
        {
            DrawableComponent = new ResistorDrawable(this, value, x, y);
        }

        public string RefName { get; set; }
        public IDrawableComponent DrawableComponent { get; set; }
    }
}