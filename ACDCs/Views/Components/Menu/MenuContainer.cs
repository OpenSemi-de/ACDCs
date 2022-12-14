using System;
using System.Collections.Generic;
using ACDCs.Views.Components.DebugView;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;
using Newtonsoft.Json;

namespace ACDCs.Views.Components.Menu;

public class MenuContainer : StackLayout
{
    public MenuContainer()
    {
        Orientation = StackOrientation.Horizontal;
        AbsoluteLayout.SetLayoutBounds(this, new(0, 0, 1, 44));
        AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.WidthProportional);
        _menuLayout = new()
        {
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill,
            Orientation = StackOrientation.Horizontal,
            Padding = 1,
        };

        _menuFrame = new()
        {
            PopupTarget = PopupTarget,
            MainContainer = this
        };

        _fileNameLabel = new Label()
        {
            Text = "New file",
            WidthRequest = 200,
            HorizontalTextAlignment = TextAlignment.Start,
            VerticalTextAlignment = TextAlignment.Center,
            Padding = 5,
            BackgroundColor = Colors.Transparent,
        };

        _menuLayout.Add(_menuFrame);
        _menuLayout.Add(_fileNameLabel);
        Add(_menuLayout);
        LoadMenu("menu_main.json");
        Loaded += MenuDragContainer_Loaded;
    }

    public DebugViewDragComtainer DebugView
    {
        get => (DebugViewDragComtainer)GetValue(DebugViewProperty);
        set => SetValue(DebugViewProperty, value);
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

    public CircuitViewContainer? CircuitView
    {
        get => (CircuitViewContainer)GetValue(CircuitViewProperty);
        set => SetValue(CircuitViewProperty, value);
    }


    public static readonly BindableProperty CircuitViewProperty =
        BindableProperty.Create(nameof(CircuitView), typeof(CircuitViewContainer), typeof(CircuitSheetPage));

    private static readonly BindableProperty DebugViewProperty =
        BindableProperty.Create(nameof(DebugView), typeof(DebugViewDragComtainer), typeof(CircuitSheetPage));

    private static readonly BindableProperty PopupTargetProperty =
                       BindableProperty.Create(nameof(PopupTarget), typeof(AbsoluteLayout), typeof(CircuitSheetPage));

    private readonly Label _fileNameLabel;
    private readonly MenuFrame _menuFrame;
    private readonly StackLayout _menuLayout;

    private async void LoadMenu(string menuMainJson)
    {
        await App.Call(async () =>
        {
            string jsonData = await App.LoadMauiAssetAsString(menuMainJson);
            List<MenuItemDefinition>? items = JsonConvert.DeserializeObject<List<MenuItemDefinition>>(jsonData);
            if (items != null) _menuFrame.LoadMenu(items, true);
        });
    }

    private void MenuDragContainer_Loaded(object? sender, EventArgs e)
    {
        if (CircuitView != null)
        {
            CircuitView.LoadedSheet += Sheet_Loaded;
            CircuitView.SavedSheet += Sheet_Saved;
        }
    }

    private void Sheet_Loaded(object? sender, EventArgs e)
    {
        _fileNameLabel.Text = CircuitView?.CurrentWorksheet.Filename;
    }

    private void Sheet_Saved(object? sender, EventArgs e)
    {
        _fileNameLabel.Text = CircuitView?.CurrentWorksheet.Filename;
    }
}
