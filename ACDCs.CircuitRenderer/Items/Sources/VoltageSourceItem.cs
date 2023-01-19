using ACDCs.CircuitRenderer.Drawables;

namespace ACDCs.CircuitRenderer.Items.Sources;

// ReSharper disable once UnusedMember.Global
public class VoltageSourceItem : SourceItem
{
    public override string DefaultValue => "5v";

    public override bool IsInsertable => true;

    public VoltageSourceItem() :
                base(SourceDrawableType.Voltage)
    {
    }
}
