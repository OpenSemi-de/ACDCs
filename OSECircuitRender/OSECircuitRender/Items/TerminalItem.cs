using OSECircuitRender.Drawables;

namespace OSECircuitRender.Items;

public sealed class TerminalItem : WorksheetItem
{
    public static string DefaultValue { get; set; } = "";

    public new static bool IsInsertable { get; set; } = true;


    public TerminalItem()
    {
        DrawableComponent = new TerminalDrawable(this, 1, 1, TerminalDrawableType.Null);
    }

    public TerminalItem(TerminalDrawableType terminalType, float x, float y)
    {
        DrawableComponent = new TerminalDrawable(this, x, y, terminalType);
    }
}