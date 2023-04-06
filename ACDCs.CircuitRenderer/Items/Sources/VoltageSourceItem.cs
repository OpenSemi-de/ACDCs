using ACDCs.CircuitRenderer.Drawables;

namespace ACDCs.CircuitRenderer.Items.Sources;

using Data.ACDCs.Components.Source;

// ReSharper disable once UnusedMember.Global
// ReSharper disable once ClassNeverInstantiated.Global
public class VoltageSourceItem : SourceItem
{
    public override string DefaultValue => "DC 5v";

    public override bool IsInsertable => true;

    public VoltageSourceItem() :
                base(SourceDrawableType.Voltage)
    {
        Model = new Source
        {
            Type = "DC",
            DcValue = 5
        };
    }
}
