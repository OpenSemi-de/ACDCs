using OSECircuitRender.Drawables;

namespace OSECircuitRender.Items;

public sealed class TerminalItem : WorksheetItem
{
    public TerminalItem()
    {
        DrawableComponent = new TerminalDrawable(this, 1, 1, TerminalDrawableType.Null);
        Value = "0";
    }

    public TerminalItem(TerminalDrawableType terminalType, float x, float y)
    {
        DrawableComponent = new TerminalDrawable(this, x, y, terminalType);
        Value = terminalType.ToString();
        if (Value == "Null")
            Value = "0";
    }

    public new static string DefaultValue { get; set; } = "";

    public new static bool IsInsertable { get; set; } = true;
}