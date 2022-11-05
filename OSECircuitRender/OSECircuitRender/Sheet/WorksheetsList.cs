﻿using System.Collections.Generic;

namespace OSECircuitRender.Sheet
{
    public sealed class WorksheetsList : List<Worksheet>
    {
        private int _sheetCount = 0;

        public int AddSheet(Worksheet sheet)
        {
            _sheetCount++;
            sheet.SheetNum = _sheetCount;
            Add(sheet);
            Log.L("Added sheet");
            return _sheetCount;
        }
    }
}