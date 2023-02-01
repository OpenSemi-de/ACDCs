using ACDCs.ApplicationLogic.Components.Circuit;
using ACDCs.ApplicationLogic.Components.Menu;
using ACDCs.ApplicationLogic.Interfaces;
using Microsoft.Maui.Layouts;
using Newtonsoft.Json;

namespace ACDCs.ApplicationLogic.Views.Menu;

#pragma warning disable IDE0065

using Sharp.UI;

#pragma warning restore IDE0065

public class MenuView : StackLayout, IMenuViewProperties
{
    private readonly StackLayout _menuLayout;

    private Label? _fileNameLabel;

    private MenuFrame? _menuFrame;

    public MenuView()
    {
        this.HorizontalOptions(LayoutOptions.Start);
        Orientation = StackOrientation.Horizontal;
        _menuLayout = new StackLayout
        {
            HorizontalOptions = LayoutOptions.Start,
            VerticalOptions = LayoutOptions.Fill,
            Orientation = StackOrientation.Horizontal,
            Padding = 1
        };

        Add(_menuLayout);
        Loaded += MenuDragContainer_Loaded;
    }

    private async void LoadMenu(string menuMainJson)
    {
        await API.Call(async () =>
        {
            string jsonData = await API.LoadMauiAssetAsString(menuMainJson);
            List<MenuItemDefinition>? items = JsonConvert.DeserializeObject<List<MenuItemDefinition>>(jsonData);
            if (items != null) _menuFrame?.LoadMenu(items, true);
        });
    }

    private void MenuDragContainer_Loaded(object? sender, EventArgs e)
    {
        _menuFrame = new MenuFrame
        {
            PopupTarget = PopupTarget,
            MainContainer = this
        };

        _menuLayout.Add(_menuFrame);

        LoadMenu(MenuFilename ?? "menu_main.json");

        if (CircuitView != null)
        {
            CircuitView.LoadedSheet += Sheet_Loaded;
            CircuitView.SavedSheet += Sheet_Saved;
        }

        _fileNameLabel = new Label
        {
            Text = "New file",
            WidthRequest = 200,
            HorizontalTextAlignment = TextAlignment.Start,
            VerticalTextAlignment = TextAlignment.Center,
            Padding = 5,
            BackgroundColor = Colors.Transparent
        };

        _menuLayout.Add(_fileNameLabel);

        AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.WidthProportional | AbsoluteLayoutFlags.PositionProportional);
        AbsoluteLayout.SetLayoutBounds(this, new Rect(0, 0, 1, 44));
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

    public CircuitView CircuitView { get; set; }
    public ComponentsView ComponentsView { get; set; }
    public string MenuFilename { get; set; }
    public AbsoluteLayout PopupTarget { get; set; }
}
