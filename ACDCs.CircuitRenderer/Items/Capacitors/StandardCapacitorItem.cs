using ACDCs.CircuitRenderer.Drawables;

namespace ACDCs.CircuitRenderer.Items.Capacitors;

// ReSharper disable once UnusedMember.Global
public class StandardCapacitorItem : CapacitorItem
{
    public override bool IsInsertable => true;

    public StandardCapacitorItem() : base("10u", CapacitorDrawableType.Standard)
    {
    }
}
