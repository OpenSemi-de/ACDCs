using OSECircuitRender.Drawables;

namespace OSECircuitRender.Items.Transistors;

public class PnpTransistorItem : TransistorItem
{
    public PnpTransistorItem() : base(TransistorDrawableType.Pnp)
    {
    }

    public new static bool IsInsertable { get; set; } = true;
}