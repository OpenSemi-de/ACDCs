using ACDCs.CircuitRenderer.Drawables;
using ACDCs.Data.ACDCs.Components;
using ACDCs.Data.ACDCs.Components.BJT;

namespace ACDCs.CircuitRenderer.Items.Transistors;

// ReSharper disable once UnusedMember.Global
public class NpnTransistorItem : TransistorItem
{
    public override bool IsInsertable => true;

    public NpnTransistorItem() : base(TransistorDrawableType.Npn)
    {
        Model = new Bjt { Type = "NPN" };
    }
}
