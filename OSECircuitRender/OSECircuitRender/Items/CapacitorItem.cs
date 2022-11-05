using OSECircuitRender.Drawables;
using OSECircuitRender.Interfaces;

namespace OSECircuitRender.Items
{
    public sealed class CapacitorItem : IWorksheetItem
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

        public string RefName { get; set; }
        public IDrawableComponent DrawableComponent { get; set; }
    }
}