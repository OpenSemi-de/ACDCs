using OSECircuitRender.Drawables;

namespace OSECircuitRender.Items
{
    public sealed class TerminalItem : WorksheetItem
    {
        public TerminalItem()
        {
            DrawableComponent = new TerminalDrawable(this, 0, 0);
        }

        public TerminalItem(TerminalDrawableType terminalType, float x, float y)
        {
            DrawableComponent = new TerminalDrawable(this, x, y, terminalType);
        }
    }
}