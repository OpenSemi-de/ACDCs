using ACDCs.CircuitRenderer.Drawables;

namespace ACDCs.CircuitRenderer.Items.Transistors;

public class PnpTransistorItem : TransistorItem
{
    public PnpTransistorItem() : base(TransistorDrawableType.Pnp)
    {
    }

    public new static bool IsInsertable { get; set; } = true;
}
