namespace ACDCs.API.Components;

using Sharp.UI;

public static class GridExtension
{
    // ReSharper disable once UnusedParameter.Global
    public static void SetRowAndColumn(this Grid grid, IView? view, int row, int column, int columnSpan = 0, int rowSpan = 0)
    {
        Grid.SetRow((BindableObject)view, row);
        Grid.SetColumn((BindableObject)view, column);
        if (columnSpan > 0)
            Grid.SetColumnSpan((BindableObject)view, columnSpan);
        if (rowSpan > 0)
            Grid.SetRowSpan((BindableObject)view, rowSpan);
    }
}
