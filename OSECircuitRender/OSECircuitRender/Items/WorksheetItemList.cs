using OSECircuitRender.Interfaces;
using OSECircuitRender.Sheet;
using System;
using System.Collections.Generic;
using OSECircuitRender.Drawables;

namespace OSECircuitRender.Items;

public sealed class WorksheetItemList : List<IWorksheetItem>
{
    public WorksheetItemList(Worksheet? worksheet)
    {
        Worksheet = worksheet;
    }

    public Action<IWorksheetItem>? OnItemAdded { get; set; }
    public ReferenceManager ReferenceManager { get; } = new();

    public Worksheet? Worksheet
    { get; set; }

    public int AddItem(IWorksheetItem item)
    {
        var refNum = ReferenceManager.GetRefNum(item.GetType().Name);
        item.RefName = $"{item.GetType().Name}{refNum}";
        Add(item);
        item.DrawableComponent.Worksheet = Worksheet;
        OnItemAdded?.Invoke(item);
        return refNum;
    }

    public NetItem AddNet(PinDrawable pin1, PinDrawable pin2)
    {
        var refNum = ReferenceManager.GetRefNum(nameof(NetItem));

        var newNet = new NetItem();
        newNet.Pins.Add(pin1);
        newNet.Pins.Add(pin2);
        newNet.RefName = $"{nameof(NetItem)}{refNum}";
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