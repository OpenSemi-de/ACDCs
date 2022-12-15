using ACDCs.CircuitRenderer.Items;
using Microsoft.Maui.Layouts;
using static Sharp.UI.AbsoluteLayout;

namespace ACDCs.Views.Components.ItemsList;

using Sharp.UI;

[BindableProperties]
public interface IItemsListDragContainerProperties
{
    AbsoluteLayout PopupTarget { get; set; }
}

[SharpObject]
public partial class ItemsListDragContainer : DragContainer.DragContainer, IItemsListDragContainerProperties
{
    public ItemsListDragContainer()
    {
        Title = "Items / selection";
        Orientation = StackOrientation.Vertical;
        ShowButtonHide();

        Microsoft.Maui.Controls.AbsoluteLayout.SetLayoutBounds(this, new(1, 100, 300, 400));
        Microsoft.Maui.Controls.AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.XProportional);

        _layout = new()
        {
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill,
            MinimumWidthRequest = 300
        };

        DataTemplate itemTemplate = new(typeof(ViewCell)) { LoadTemplate = LoadTemplate };

        DataTemplate headerTemplate = new(typeof(ViewCell)) { LoadTemplate = LoadHeader };

        _listViewItems = new()
        {
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill,
            ItemTemplate = itemTemplate,
            HeaderTemplate = headerTemplate,
            Header = LoadHeader(),
            ItemsSource = null,
            MinimumWidthRequest = 300
        };

        _listViewItems.ItemsSource = new List<ItemsListItem>();

        _layout.Add(_listViewItems);
        Layout = _layout;
#pragma warning disable CS8974
        App.Com<Action<WorksheetItemList, WorksheetItemList>>("ItemList", "SetItems", SetItems);
#pragma warning restore CS8974
    }

    public void SetItems(WorksheetItemList items, WorksheetItemList selected)
    {
        List<ItemsListItem> list = items.Select(item =>
            new ItemsListItem(selected.Contains(item), item.GetType().Name.Replace("Item", ""), item.RefName, item)
                               ).ToList();
        _listViewItems.ItemsSource = null;
        _listViewItems.ItemsSource = list;
    }


    private readonly StackLayout _layout;
    private readonly ListView _listViewItems;

    private void AddLabelToTemplate(StackLayout layout, string bindingPath, double width)
    {
        layout.Add(new Label().BindingContext(bindingPath).WidthRequest(width));
    }

    private object LoadHeader()
    {
        StackLayout layout = new() { Orientation = StackOrientation.Horizontal };
        layout.Add(new Label().Text("Type").WidthRequest(120));
        layout.Add(new Label().Text("Name").WidthRequest(160));
        return layout;
    }

    private object LoadTemplate()
    {
        ViewCell viewcell = new();
        StackLayout layout = new() { Orientation = StackOrientation.Horizontal };
        viewcell.View = layout;
        AddLabelToTemplate(layout, "TypeName", 120);
        AddLabelToTemplate(layout, "RefName", 160);
        return viewcell;
    }
}
