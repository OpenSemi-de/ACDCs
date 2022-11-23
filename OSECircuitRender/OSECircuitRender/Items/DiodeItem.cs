using System.ComponentModel;
using Newtonsoft.Json.Linq;
using OSECircuitRender.Drawables;

namespace OSECircuitRender.Items;

public sealed class DiodeItem : WorksheetItem
{
    public DiodeItem()
    {
        DrawableComponent = new DiodeDrawable(this, DefaultValue, 1,1);
    }

    public string DefaultValue { get; set; } = "";

    public new static bool IsInsertable { get; set; } = true;

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

    public string Value { get; set; }
}