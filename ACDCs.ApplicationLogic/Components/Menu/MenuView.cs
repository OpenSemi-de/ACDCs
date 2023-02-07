﻿namespace ACDCs.ApplicationLogic.Components.Menu;

using Circuit;
using Microsoft.Maui.Layouts;
using Newtonsoft.Json;
using Sharp.UI;
using Window = Window.Window;

public class MenuView : ContentView
{
    private readonly StackLayout _menuLayout;
    private readonly Dictionary<string, object> _menuParameters;
    private Label? _fileNameLabel;

    private MenuFrame? _menuFrame;

    // ReSharper disable once MemberCanBePrivate.Global
    public CircuitView? CircuitView { get; set; }

    public string? MenuFilename { get; set; }

    public Window? ParentWindow { get; set; }

    public MenuView(string? menuFile, Dictionary<string, object> menuParameters)
    {
        _menuParameters = menuParameters;
        if (menuFile != null)
        {
            MenuFilename = menuFile;
        }

        this.HorizontalOptions(LayoutOptions.Start);
        _menuLayout = new StackLayout
        {
            HorizontalOptions = LayoutOptions.Start,
            VerticalOptions = LayoutOptions.Fill,
            Orientation = StackOrientation.Horizontal,
            Padding = 0,
            Margin = 0
        };

        this.BackgroundColor(Colors.Transparent);
        Content = _menuLayout;
        Loaded += MenuDragContainer_Loaded;
    }

    public MenuView()
    {
    }

    private async void LoadMenu(string? menuMainJson)
    {
        await API.Call(async () =>
        {
            if (menuMainJson == "") return;
            string jsonData = await API.LoadMauiAssetAsString(menuMainJson);
            List<MenuItemDefinition>? items = JsonConvert.DeserializeObject<List<MenuItemDefinition>>(jsonData);
            if (items != null) _menuFrame?.LoadMenu(items, true, _menuParameters);
        });
    }

    private void MenuDragContainer_Loaded(object? sender, EventArgs e)
    {
        _menuFrame = new MenuFrame
        {
            ParentWindow = ParentWindow
        };

        _menuLayout.Add(_menuFrame);

        LoadMenu(MenuFilename);

        if (CircuitView != null)
        {
            CircuitView.LoadedSheet += Sheet_Loaded;
            CircuitView.SavedSheet += Sheet_Saved;
        }

        _fileNameLabel = new Label
        {
            Text = "",
            WidthRequest = 200,
            HorizontalTextAlignment = TextAlignment.Start,
            VerticalTextAlignment = TextAlignment.Center,
            Padding = 5,
            BackgroundColor = Colors.Transparent
        };

        _menuLayout.Add(_fileNameLabel);

        AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.WidthProportional);
        AbsoluteLayout.SetLayoutBounds(this, new Rect(2, 0, 1, 34));
    }

    private void Sheet_Loaded(object? sender, EventArgs e)
    {
        if (_fileNameLabel != null)
        {
            _fileNameLabel.Text = CircuitView?.CurrentWorksheet.Filename;
        }
    }

    private void Sheet_Saved(object? sender, EventArgs e)
    {
        if (_fileNameLabel != null)
        {
            _fileNameLabel.Text = CircuitView?.CurrentWorksheet.Filename;
        }
    }
}
