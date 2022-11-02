using System.Collections.Generic;

namespace OSECircuitRender
{
    public sealed class WorksheetItems : List<IWorksheetItem>
    {
        public ReferenceManager ReferenceManager { get; } = new ReferenceManager();

        public int AddItem(IWorksheetItem item)
        {
            int refNum = ReferenceManager.GetRefNum(item.GetType().Name);
            item.RefName = $"{item.GetType().Name}{refNum}";
            base.Add(item);
            return refNum;
        }
    }
}