using OSECircuitRender.Drawables;

namespace OSECircuitRender.Items
{
    public sealed class ResistorItem : WorksheetItem
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
    }
}