using ACDCs.CircuitRenderer.Drawables;

namespace ACDCs.CircuitRenderer.Items.Transistors;

public class PnpTransistorItem : TransistorItem
{
    public PnpTransistorItem() : base(TransistorDrawableType.Pnp)
    {
    }

    public override bool IsInsertable => true;
}
