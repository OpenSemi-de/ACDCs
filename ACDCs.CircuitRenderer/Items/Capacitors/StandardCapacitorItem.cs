using OSECircuitRender.Drawables;

namespace OSECircuitRender.Items.Capacitors;

public class StandardCapacitorItem : CapacitorItem
{
    public StandardCapacitorItem() : base("10u", CapacitorDrawableType.Standard)
    {
    }

    public new static bool IsInsertable { get; set; } = true;
}