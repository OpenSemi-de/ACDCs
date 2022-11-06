using OSECircuitRender.Drawables;
using OSECircuitRender.Interfaces;

namespace OSECircuitRender.Items
{
    public sealed class InductorItem : WorksheetItem
    {
        public InductorItem()
        {
            DrawableComponent = new InductorDrawable(this);
        }

        public InductorItem(string value, float x, float y)
        {
            DrawableComponent = new InductorDrawable(this, value, x, y);
        }
    }
}