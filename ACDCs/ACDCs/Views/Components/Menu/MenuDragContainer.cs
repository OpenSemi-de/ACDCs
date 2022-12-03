using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ACDCs.Views.Components.Menu;

public class MenuDragContainer : DragContainer.DragContainer
{
    private static readonly BindableProperty PopupTargetProperty =
        BindableProperty.Create(nameof(PopupTarget), typeof(AbsoluteLayout), typeof(CircuitSheetPage));

    private readonly MenuFrame _menuFrame;

    public MenuDragContainer()
    {
        Orientation = StackOrientation.Horizontal;
        AbsoluteLayout.SetLayoutBounds(this, new(0, 0, 1, 50));
        AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.WidthProportional);
        _menuFrame = new()
        {
            PopupTarget = PopupTarget,
            MainContainer = this
        };
        Layout = _menuFrame;
        LoadMenu("menu_main.json");
    }

    public AbsoluteLayout PopupTarget
    {
        get => (AbsoluteLayout)GetValue(PopupTargetProperty);

        set
        {
            SetValue(PopupTargetProperty, value);
            _menuFrame.PopupTarget = value;
        }
    }

    private async void LoadMenu(string menuMainJson)
    {
        await App.Call(async () =>
        {
            var jsonData = await App.LoadMauiAssetAsString(menuMainJson);
            var items = JsonConvert.DeserializeObject<List<MenuItemDefinition>>(jsonData);
            if (items != null) _menuFrame.LoadMenu(items, true);
        });
    }
}