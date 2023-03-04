namespace ACDCs.API.Core.Components;

using ObjectsComparer;

public static class CompareExtension
{
    public static bool IsFlatEqual<T>(this T? left, T? right)
    {
        if (left == null || right == null)
            return false;

        Comparer<T> comparer = new();
        bool isEqual = comparer.Compare(left, right);
        return isEqual;
    }
}
