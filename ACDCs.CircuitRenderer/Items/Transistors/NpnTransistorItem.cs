using ACDCs.CircuitRenderer.Drawables;

namespace ACDCs.CircuitRenderer.Items.Transistors;

public class NpnTransistorItem : TransistorItem
{
    public NpnTransistorItem() : base(TransistorDrawableType.Npn)
    {
    }

    public override bool IsInsertable => true;
}
