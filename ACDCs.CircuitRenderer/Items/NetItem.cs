namespace ACDCs.CircuitRenderer.Items;

using Drawables;

public class NetItem : WorksheetItem
{
    public new DrawablePinList Pins { get; } = new();
}
