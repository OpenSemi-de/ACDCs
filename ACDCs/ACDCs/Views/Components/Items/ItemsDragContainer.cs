using System.Collections.Generic;
using ACDCs.Views.Components.Menu;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;
using Newtonsoft.Json;

namespace ACDCs.Views.Components.Items;

public class ItemsDragContainer: DragContainer.DragContainer
{
    private static readonly BindableProperty PopupTargetProperty = BindableProperty.Create(nameof(PopupTarget), typeof(AbsoluteLayout), typeof(CircuitSheetPage));
    private readonly StackLayout _layout;

    public ItemsDragContainer()
    {
        Title = "Items";
        Orientation = StackOrientation.Horizontal;
        AbsoluteLayout.SetLayoutBounds(this, new Rect(0,1,1,60));
        AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.WidthProportional | AbsoluteLayoutFlags.YProportional);
        _layout = new StackLayout();
        Layout = _layout;
    }

    public AbsoluteLayout PopupTarget
    {
        get => (AbsoluteLayout)GetValue(PopupTargetProperty);

        set
        {
            SetValue(PopupTargetProperty, value);
        }
    }
}