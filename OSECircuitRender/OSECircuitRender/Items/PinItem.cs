using OSECircuitRender.Drawables;
using OSECircuitRender.Interfaces;

namespace OSECircuitRender.Items
{
    public sealed class PinItem : WorksheetItem
    {
        public PinItem()
        {
            DrawableComponent = new PinDrawable(this, 0, 0);
        }

        public PinItem(PinDrawableType pinType, float x, float y)
        {
            DrawableComponent = new PinDrawable(this, x, y, pinType);
        }
    }
}