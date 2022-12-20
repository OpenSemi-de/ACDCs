using ACDCs.CircuitRenderer.Interfaces;

namespace ACDCs.Views.Components.ItemsList;

public class ItemsListItem
{
    public bool IsSelected { get; set; }

    public IWorksheetItem Item { get; set; }

    public string RefName { get; set; }

    public string TypeName { get; set; }

    public ItemsListItem(bool isSelected, string typeName, string refName, IWorksheetItem item)
    {
        IsSelected = isSelected;
        TypeName = typeName;
        RefName = refName;
        Item = item;
    }
}
