using ACDCs.CircuitRenderer.Drawables;

namespace ACDCs.CircuitRenderer.Items.Transistors;

public class NpnTransistorItem : TransistorItem
{
    public override bool IsInsertable => true;

    public NpnTransistorItem() : base(TransistorDrawableType.Npn)
    {
    }
}
