using OSECircuitRender.Drawables;

namespace OSECircuitRender.Items;

public sealed class DiodeItem : WorksheetItem
{
    public DiodeItem()
    {
        DrawableComponent = new DiodeDrawable(this);
    }

    public DiodeItem(string value, float x, float y)
    {
        DrawableComponent = new DiodeDrawable(this, value, x, y);
        Value = value;
    }

    public string Value { get; set; }
}