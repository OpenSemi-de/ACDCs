using System.Collections.Generic;

namespace OSECircuitRender
{
    public sealed class Worksheets : List<Worksheet>
    {
        private int sheetCount = 0;

        public int AddSheet(Worksheet sheet)
        {
            sheetCount++;
            sheet.SheetNum = sheetCount;
            base.Add(sheet);
            Log.L("Added sheet");
            return sheetCount;
        }
    }
}