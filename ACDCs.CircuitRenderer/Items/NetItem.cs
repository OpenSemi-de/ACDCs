namespace ACDCs.CircuitRenderer.Items;

using System;
using System.Collections.Generic;

public class NetItem : WorksheetItem
{
    public new List<Guid> Pins { get; } = new();
}
