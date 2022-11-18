using System;
using System.Collections.Generic;
using OSECircuitRender.Interfaces;
using OSECircuitRender.Sheet;

namespace OSECircuitRender.Items;

public sealed class WorksheetItemList : List<IWorksheetItem>
{
    public Action<IWorksheetItem> OnItemAdded { get; set; }
    public ReferenceManager ReferenceManager { get; } = new();

    public int AddItem(IWorksheetItem item)
    {
        var refNum = ReferenceManager.GetRefNum(item.GetType().Name);
        item.RefName = $"{item.GetType().Name}{refNum}";
        Add(item);
        OnItemAdded?.Invoke(item);
        return refNum;
    }

    public void OnAdded(Action<IWorksheetItem> onItemAdded)
    {
        OnItemAdded = onItemAdded;
    }
}