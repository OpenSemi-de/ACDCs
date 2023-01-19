using ACDCs.CircuitRenderer.Drawables;

namespace ACDCs.CircuitRenderer.Items.Capacitors;

// ReSharper disable once UnusedMember.Global
public sealed class PolarizedCapacitorItem : CapacitorItem
{
    public override string DefaultValue => "";

    public override bool IsInsertable => true;

    public PolarizedCapacitorItem() : base("10u", CapacitorDrawableType.Polarized)
    {
    }
}
