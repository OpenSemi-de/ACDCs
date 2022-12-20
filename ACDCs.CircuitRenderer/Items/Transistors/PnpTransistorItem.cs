using ACDCs.CircuitRenderer.Drawables;

namespace ACDCs.CircuitRenderer.Items.Transistors;

public class PnpTransistorItem : TransistorItem
{
    public override bool IsInsertable => true;

    public PnpTransistorItem() : base(TransistorDrawableType.Pnp)
    {
    }
}
