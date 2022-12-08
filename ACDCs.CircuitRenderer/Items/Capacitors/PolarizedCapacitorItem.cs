using ACDCs.CircuitRenderer.Drawables;

namespace ACDCs.CircuitRenderer.Items.Capacitors;

public sealed class PolarizedCapacitorItem : CapacitorItem
{
    public PolarizedCapacitorItem() : base("10u", CapacitorDrawableType.Polarized)
    {
    }

    public new static bool IsInsertable { get; set; } = true;
}
