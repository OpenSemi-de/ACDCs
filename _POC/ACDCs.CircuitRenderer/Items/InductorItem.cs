using ACDCs.CircuitRenderer.Drawables;
using ACDCs.Data.ACDCs.Components.Inductor;

namespace ACDCs.CircuitRenderer.Items;

public sealed class InductorItem : WorksheetItem
{
    public override string DefaultValue => "1m";

    public override bool IsInsertable => true;

    public InductorItem()
    {
        DrawableComponent = new InductorDrawable(this, DefaultValue, 1, 1);
        Value = DefaultValue;
        Model = new Inductor();
    }

    public InductorItem(string value, float x, float y)
    {
        Value = value;
        DrawableComponent = new InductorDrawable(this, value, x, y);
        Model = new Inductor();
    }

    public InductorItem(string value)
    {
        Value = value;
        DrawableComponent = new InductorDrawable(this, value, 1, 1);
        Model = new Inductor();
    }
}
