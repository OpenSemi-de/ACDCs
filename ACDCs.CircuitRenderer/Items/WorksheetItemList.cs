using System;
using System.Collections.Generic;
using ACDCs.CircuitRenderer.Drawables;
using ACDCs.CircuitRenderer.Interfaces;
using ACDCs.CircuitRenderer.Sheet;

namespace ACDCs.CircuitRenderer.Items;

public sealed class WorksheetItemList : List<IWorksheetItem>
{
    public Action<IWorksheetItem>? OnItemAdded { get; set; }

    public ReferenceManager ReferenceManager { get; } = new();

    public Worksheet? Worksheet
    { get; set; }

    public WorksheetItemList(Worksheet? worksheet)
    {
        Worksheet = worksheet;
    }

    public int AddItem(IWorksheetItem item)
    {
        int refNum = ReferenceManager.GetRefNum(item.GetType().Name);
        item.RefName = $"{item.GetType().Name.Replace("Item", "")}{refNum}";
        Add(item);
        item.DrawableComponent.Worksheet = Worksheet;
        OnItemAdded?.Invoke(item);
        return refNum;
    }

    public NetItem AddNet(PinDrawable pin1, PinDrawable pin2)
    {
        int refNum = ReferenceManager.GetRefNum(nameof(NetItem));

        NetItem newNet = new();
        newNet.Pins.Add(pin1);
        newNet.Pins.Add(pin2);
        newNet.RefName = $"{nameof(NetItem).Replace("Item", "")}{refNum}";
        newNet.DrawableComponent.Worksheet = Worksheet;
        Add(newNet);
        OnItemAdded?.Invoke(newNet);
        return newNet;
    }

    public void OnAdded(Action<IWorksheetItem>? onItemAdded)
    {
        OnItemAdded = onItemAdded;
    }
}
