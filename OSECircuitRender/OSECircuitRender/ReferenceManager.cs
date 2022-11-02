using System.Collections.Generic;

namespace OSECircuitRender
{
    public sealed class ReferenceManager
    {
        private Dictionary<string, int> refCounts = new();

        public int GetRefNum(string type)
        {
            if (!refCounts.ContainsKey(type))
            {
                refCounts.Add(type, 0);
            }
            refCounts[type]++;
            return refCounts[type];
        }
    }
}