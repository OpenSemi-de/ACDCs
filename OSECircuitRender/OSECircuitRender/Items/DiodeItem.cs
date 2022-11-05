using OSECircuitRender.Drawables;
using OSECircuitRender.Interfaces;

namespace OSECircuitRender.Items
{
    public sealed class DiodeItem : IWorksheetItem
    {
        public string Value { get; set; }

        public DiodeItem()
        {
            DrawableComponent = new DiodeDrawable(this);
        }

        public DiodeItem(string value, float x, float y)
        {
            DrawableComponent = new DiodeDrawable(this, value, x, y);
            Value = value;
        }

        public string RefName { get; set; }
        public IDrawableComponent DrawableComponent { get; set; }
    }
}