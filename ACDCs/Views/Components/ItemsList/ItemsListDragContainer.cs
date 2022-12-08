using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;
using System;
using System.Collections.Generic;
using System.Linq;
using ACDCs.CircuitRenderer.Items;

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
        ShowButtonHide();

        AbsoluteLayout.SetLayoutBounds(this, new(1, 100, 300, 400));
        AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.XProportional);

        _layout = new()
        {
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill,
            MinimumWidthRequest = 300
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
            ItemsSource = null,
            MinimumWidthRequest = 300
        };

        _listViewItems.ItemsSource = new List<ItemsListItem>();

        _layout.Add(_listViewItems);
        Layout = _layout;
#pragma warning disable CS8974 // Converting method group to non-delegate type
        App.Com<Action<WorksheetItemList, WorksheetItemList>>("ItemList", "SetItems", SetItems);
#pragma warning restore CS8974 // Converting method group to non-delegate type
    }

    public AbsoluteLayout PopupTarget
    {
        get => (AbsoluteLayout)GetValue(PopupTargetProperty);

        set => SetValue(PopupTargetProperty, value);
    }

    public void SetItems(WorksheetItemList items, WorksheetItemList selected)
    {
        var list = items.Select(item =>
            new ItemsListItem(selected.Contains(item), item.GetType().Name.Replace("Item", ""), item.RefName, item)
        ).ToList();
        _listViewItems.ItemsSource = null;
        _listViewItems.ItemsSource = list;
    }

    private void AddLabelToTemplate(StackLayout layout, string bindingPath, double width)
    {
        layout.Add(new Label().Bind(bindingPath).Width(width));
    }

    private object LoadHeader()
    {
        var layout = new StackLayout
        {
            Orientation = StackOrientation.Horizontal
        };
        layout.Add(new Label().Text("Type").Width(120));
        layout.Add(new Label().Text("Name").Width(160));
        return layout;
    }

    private object LoadTemplate()
    {
        var viewcell = new ViewCell();
        var layout = new StackLayout
        {
            Orientation = StackOrientation.Horizontal
        };
        viewcell.View = layout;
        AddLabelToTemplate(layout, "TypeName", 120);
        AddLabelToTemplate(layout, "RefName", 160);
        return viewcell;
    }
}
