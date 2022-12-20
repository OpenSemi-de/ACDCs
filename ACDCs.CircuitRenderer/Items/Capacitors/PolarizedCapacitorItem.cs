using ACDCs.CircuitRenderer.Drawables;

namespace ACDCs.CircuitRenderer.Items.Capacitors;

public sealed class PolarizedCapacitorItem : CapacitorItem
{
    public override string DefaultValue => "";

    public override bool IsInsertable => true;

    public PolarizedCapacitorItem() : base("10u", CapacitorDrawableType.Polarized)
    {
    }
}
