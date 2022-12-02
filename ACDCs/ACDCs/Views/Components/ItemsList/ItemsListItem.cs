using OSECircuitRender.Interfaces;

namespace ACDCs.Views.Components.ItemsList;

public class ItemsListItem
{
    public ItemsListItem(bool isSelected, string typeName, string refName, IWorksheetItem item)
    {
        IsSelected = isSelected;
        TypeName = typeName;
        RefName = refName;
        Item = item;
    }

    public bool IsSelected { get; set; }
    public string TypeName { get; set; }
    public string RefName { get; set; }
    public IWorksheetItem Item { get; set; }
}