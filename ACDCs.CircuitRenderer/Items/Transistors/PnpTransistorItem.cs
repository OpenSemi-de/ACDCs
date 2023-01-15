using ACDCs.CircuitRenderer.Drawables;
using ACDCs.Data.ACDCs.Components;

namespace ACDCs.CircuitRenderer.Items.Transistors;

public class PnpTransistorItem : TransistorItem
{
    public override bool IsInsertable => true;

    public PnpTransistorItem() : base(TransistorDrawableType.Pnp)
    {
        this.Model = new Bjt() { Type = "PNP" };
    }
}
