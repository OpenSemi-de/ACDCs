using ACDCs.CircuitRenderer.Drawables;

namespace ACDCs.CircuitRenderer.Items;

public sealed class TerminalItem : WorksheetItem
{
    public override string DefaultValue => "";

    public override bool IsInsertable => true;

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
}
