using System;
using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;
using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.Maui;

namespace ACDCs.Views.Components.Menu;

public class MenuDragContainer : DragContainer.DragContainer
{
    private static readonly BindableProperty PopupTargetProperty =
        BindableProperty.Create(nameof(PopupTarget), typeof(AbsoluteLayout), typeof(CircuitSheetPage));

    private readonly StackLayout _menuLayout;
    private readonly MenuFrame _menuFrame;
    private readonly Label _fileNameLabel;

    public MenuDragContainer()
    {
        Orientation = StackOrientation.Horizontal;
        AbsoluteLayout.SetLayoutBounds(this, new(0, 0, 1, 50));
        AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.WidthProportional);
        _menuLayout = new()
        {
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill,
            Orientation = StackOrientation.Horizontal,
            Padding = 5,
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
        Layout = _menuLayout;
        LoadMenu("menu_main.json");
        Loaded += MenuDragContainer_Loaded;

    }

    private void MenuDragContainer_Loaded(object? sender, System.EventArgs e)
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