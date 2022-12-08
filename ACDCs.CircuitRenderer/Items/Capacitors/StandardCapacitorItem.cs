using ACDCs.CircuitRenderer.Drawables;

namespace ACDCs.CircuitRenderer.Items.Capacitors;

public class StandardCapacitorItem : CapacitorItem
{
    public StandardCapacitorItem() : base("10u", CapacitorDrawableType.Standard)
    {
    }

    public override bool IsInsertable => true;
}
