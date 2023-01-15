using ACDCs.CircuitRenderer.Drawables;
using ACDCs.Data.ACDCs.Components;

namespace ACDCs.CircuitRenderer.Items.Transistors;

public class NpnTransistorItem : TransistorItem
{
    public override bool IsInsertable => true;

    public NpnTransistorItem() : base(TransistorDrawableType.Npn)
    {
        this.Model = new Bjt() { Type = "NPN" };
    }
}
