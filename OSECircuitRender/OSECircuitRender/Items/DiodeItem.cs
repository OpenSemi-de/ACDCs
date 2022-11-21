using Newtonsoft.Json.Linq;
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

    public DiodeItem(string value)
    {
        DrawableComponent = new DiodeDrawable(this, value, 0, 0);
        Value = value;
    }

    public string Value { get; set; }
}