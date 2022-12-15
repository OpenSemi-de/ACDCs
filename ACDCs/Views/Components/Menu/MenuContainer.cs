using ACDCs.Views.Components.CircuitView;
using ACDCs.Views.Components.DebugView;
using Microsoft.Maui.Layouts;
using Newtonsoft.Json;

namespace ACDCs.Views.Components.Menu;

using Sharp.UI;

[BindableProperties]
public interface IMenuContainer
{
    DebugViewDragComtainer DebugView { get; set; }
    AbsoluteLayout PopupTarget { get; set; }
    CircuitViewContainer CircuitView { get; set; }
}

[SharpObject]
public partial class MenuContainer : StackLayout, IMenuContainer
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
        Add(_menuLayout);
        Loaded += MenuDragContainer_Loaded;
    }

    private readonly Label _fileNameLabel;
    private MenuFrame _menuFrame;
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
        _menuFrame = new()
        {
            PopupTarget = PopupTarget,
            MainContainer = this
        };

        _menuLayout.Add(_menuFrame);

        LoadMenu("menu_main.json");

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
