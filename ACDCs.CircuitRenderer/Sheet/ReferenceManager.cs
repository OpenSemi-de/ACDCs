using System.Collections.Generic;

namespace ACDCs.CircuitRenderer.Sheet;

public sealed class ReferenceManager
{
    public int GetRefNum(string type)
    {
        if (!_refCounts.ContainsKey(type)) _refCounts.Add(type, 0);
        _refCounts[type]++;
        return _refCounts[type];
    }

    private readonly Dictionary<string, int> _refCounts = new();
}
