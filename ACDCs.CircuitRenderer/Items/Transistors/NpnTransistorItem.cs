using ACDCs.CircuitRenderer.Drawables;

namespace ACDCs.CircuitRenderer.Items.Transistors;

public class NpnTransistorItem : TransistorItem
{
    public NpnTransistorItem() : base(TransistorDrawableType.Npn)
    {
    }

    public new static bool IsInsertable { get; set; } = true;
}
