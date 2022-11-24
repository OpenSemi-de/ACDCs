using OSECircuitRender.Drawables;

namespace OSECircuitRender.Items.Transistors;

public class NpnTransistorItem : TransistorItem
{
    public NpnTransistorItem() : base(TransistorDrawableType.Npn)
    {
    }

    public new static bool IsInsertable { get; set; } = true;
}