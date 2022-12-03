using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;
using OSECircuitRender.Items;
using System.Collections.Generic;
using System.Linq;

namespace ACDCs.Views.Components.ItemsList;

public class ItemsListDragContainer : DragContainer.DragContainer
{
    private static readonly BindableProperty PopupTargetProperty =
        BindableProperty.Create(nameof(PopupTarget), typeof(AbsoluteLayout), typeof(CircuitSheetPage));

    private readonly StackLayout _layout;
    private readonly ListView _listViewItems;

    public ItemsListDragContainer()
    {
        Title = "Items / selection";
        Orientation = StackOrientation.Vertical;
     
        AbsoluteLayout.SetLayoutBounds(this, new(1, 100, 300, 400));
        AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.XProportional);
     
        _layout = new()
        {
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill
        };

        DataTemplate itemTemplate = new(typeof(ViewCell))
        {
            LoadTemplate = LoadTemplate
        };

        DataTemplate headerTemplate = new(typeof(ViewCell))
        {
            LoadTemplate = LoadHeader
        };
    
        _listViewItems = new(ListViewCachingStrategy.RecycleElement)
        {
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill,
            ItemTemplate = itemTemplate,
            HeaderTemplate = headerTemplate,
            Header = LoadHeader(),
            ItemsSource = null
        };

        _listViewItems.ItemsSource = new List<ItemsListItem>();

        _layout.Add(_listViewItems);
        Layout = _layout;
    }

    private object LoadHeader()
    {
        var layout = new StackLayout
        {
            Orientation = StackOrientation.Horizontal
        };
        layout.Add(new Label().Text("Type"));
        layout.Add(new Label().Text("Name"));
        return layout;
    }

    private object LoadTemplate()
    {
        var layout = new StackLayout
        {
            Orientation = StackOrientation.Horizontal
        };
        AddLabelToTemplate(layout, "TypeName");
        AddLabelToTemplate(layout, "RefName");
        return layout;
    }

    private void AddLabelToTemplate(StackLayout layout, string bindingPath)
    {
        layout.Add(new Label().Bind(bindingPath));
    }

    public void SetItems(WorksheetItemList items, WorksheetItemList selected)
    {
        var list = items.Select(item =>
            new ItemsListItem(selected.Contains(item), item.GetType().Name.Replace("Item", ""), item.RefName, item)
        ).ToList();
        _listViewItems.ItemsSource = null;
        _listViewItems.ItemsSource = list;
    }

    public AbsoluteLayout PopupTarget
    {
        get => (AbsoluteLayout)GetValue(PopupTargetProperty);

        set => SetValue(PopupTargetProperty, value);
    }
}