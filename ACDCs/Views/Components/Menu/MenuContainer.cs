using ACDCs.Views.Components.CircuitView;
using ACDCs.Views.Components.DebugView;
using Microsoft.Maui.Layouts;
using Newtonsoft.Json;

namespace ACDCs.Views.Components.Menu;

using Sharp.UI;

[BindableProperties]
public interface IMenuContainer
{
    CircuitViewContainer CircuitView { get; set; }
    ComponentsView ComponentsView { get; set; }
    DebugViewDragComtainer DebugView { get; set; }
    string MenuFilename { get; set; }
    AbsoluteLayout PopupTarget { get; set; }
}

[SharpObject]
public partial class MenuContainer : StackLayout, IMenuContainer
{
    private readonly StackLayout _menuLayout;

    private Label _fileNameLabel;

    private MenuFrame _menuFrame;

    public MenuContainer()
    {
        this.HorizontalOptions(LayoutOptions.Start);
        Orientation = StackOrientation.Horizontal;
        _menuLayout = new()
        {
            HorizontalOptions = LayoutOptions.Start,
            VerticalOptions = LayoutOptions.Fill,
            Orientation = StackOrientation.Horizontal,
            Padding = 1,
        };

        Add(_menuLayout);
        Loaded += MenuDragContainer_Loaded;
    }

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
        _menuFrame = new()
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

        _fileNameLabel = new Label()
        {
            Text = "New file",
            WidthRequest = 200,
            HorizontalTextAlignment = TextAlignment.Start,
            VerticalTextAlignment = TextAlignment.Center,
            Padding = 5,
            BackgroundColor = Colors.Transparent,
        };

        _menuLayout.Add(_fileNameLabel);

        AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.WidthProportional | AbsoluteLayoutFlags.PositionProportional);
        AbsoluteLayout.SetLayoutBounds(this, new(0, 0, 1, 44));
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
