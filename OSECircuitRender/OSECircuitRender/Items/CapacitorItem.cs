using OSECircuitRender.Drawables;

namespace OSECircuitRender.Items
{
    public sealed class CapacitorItem : WorksheetItem
    {
        public string Value { get; set; }

        public CapacitorItem()
        {
            DrawableComponent = new CapacitorDrawable(this);
        }

        public CapacitorItem(string value, CapacitorDrawableType type, float x, float y)
        {
            DrawableComponent = new CapacitorDrawable(this, value, type, x, y);
            Value = value;
        }
    }
}