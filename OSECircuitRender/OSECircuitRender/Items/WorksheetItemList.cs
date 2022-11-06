using System.Collections.Generic;
using OSECircuitRender.Interfaces;
using OSECircuitRender.Sheet;

namespace OSECircuitRender.Items
{
    public sealed class WorksheetItemList : List<IWorksheetItem>
    {
        public ReferenceManager ReferenceManager { get; } = new();

        public int AddItem(IWorksheetItem item)
        {
            int refNum = ReferenceManager.GetRefNum(item.GetType().Name);
            item.RefName = $"{item.GetType().Name}{refNum}";
            Add(item);
            return refNum;
        }
    }
}