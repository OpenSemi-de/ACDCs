using System.Collections.Generic;

namespace ACDCs.CircuitRenderer.Sheet;

public sealed class WorksheetsList : List<Worksheet>
{
    private int _sheetCount;

    public int AddSheet(Worksheet sheet)
    {
        _sheetCount++;
        sheet.SheetNum = _sheetCount;
        Add(sheet);
        Log.L("Added sheet");
        return _sheetCount;
    }
}
