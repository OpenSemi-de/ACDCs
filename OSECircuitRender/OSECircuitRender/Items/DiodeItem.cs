using OSECircuitRender.Drawables;

namespace OSECircuitRender.Items;

public sealed class DiodeItem : WorksheetItem
{
    public DiodeItem()
    {
        DrawableComponent = new DiodeDrawable(this, DefaultValue, 1, 1);
        Value = DefaultValue;
    }

    public DiodeItem(string value, float x, float y)
    {
        DrawableComponent = new DiodeDrawable(this, value, x, y);
        Value = value;
    }

    public DiodeItem(string value)
    {
        DrawableComponent = new DiodeDrawable(this, value, 1, 1);
        Value = value;
    }

    public new static bool IsInsertable { get; set; } = true;
    public new string DefaultValue { get; set; } = "0.7";

}