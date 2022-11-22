using OSECircuitRender.Drawables;
using OSECircuitRender.Interfaces;

namespace OSECircuitRender.Items.Capacitors;

public sealed class PolarizedCapacitorItem : CapacitorItem
{
    public PolarizedCapacitorItem() : base("10u", CapacitorDrawableType.Polarized)
    {

    }
    public new static bool IsInsertable { get; set; } = true;
}