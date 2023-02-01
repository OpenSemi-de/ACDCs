using ACDCs.ApplicationLogic.Components.Menu;
using ACDCs.ApplicationLogic.Components.Menu.MenuHandlers;
using ACDCs.ApplicationLogic.Views.Edit;
using ACDCs.ApplicationLogic.Views.Menu;
using ACDCs.ApplicationLogic.Views.Properties;
using Microsoft.Maui.Layouts;

namespace ACDCs.ApplicationLogic.Components.Circuit;

#pragma warning disable IDE0065

using Sharp.UI;
using ItemsView = ACDCs.ApplicationLogic.Views.Items.ItemsView;

#pragma warning restore IDE0065

public sealed class CircuitSheetView : AbsoluteLayout
{
    private readonly CircuitView _circuitView;

    private readonly Label? _cursorDebugLabel;
    private readonly Views.Items.ItemsView _itemsView;

    // ReSharper disable once NotAccessedField.Local
    private readonly List<MenuHandler> _menuHandlers;

    private readonly MenuView _menuView;

    private readonly bool _showCursorDebugOutput = Convert.ToBoolean(API.GetPreference("ShowDebugCursorOutput"));

    // ReSharper disable once NotAccessedField.Local
    private EditView? _editWindow;

    private PropertiesView? _propertiesWindow;

    public CircuitSheetView()
    {
        _itemsView = new ItemsView
        {
            ButtonWidth = 60,
            ButtonHeight = 60,
            ZIndex = 10
        };

        AbsoluteLayout.SetLayoutFlags(_itemsView, AbsoluteLayoutFlags.WidthProportional | AbsoluteLayoutFlags.YProportional);
        AbsoluteLayout.SetLayoutBounds(_itemsView, new Rect(0, 1, 1, 60));
        Add(_itemsView);

        _circuitView = new CircuitView();
        _circuitView.PopupTarget = this;
        _circuitView.ZIndex = 0;

        AbsoluteLayout.SetLayoutFlags(_circuitView, AbsoluteLayoutFlags.SizeProportional);
        AbsoluteLayout.SetLayoutBounds(_circuitView, new Rect(0, 0, 1, 1));
        Add(_circuitView);

        _menuHandlers = new List<MenuHandler>
        {
            new FileMenuHandlers
            {
                CircuitView = _circuitView,
                PopupPage = API.MainPage
            },
            new EditMenuHandlers
            {
                CircuitView = _circuitView
            },
            new InfoMenuHandlers
            {
                CircuitView = _circuitView
            }
        };

        if (_showCursorDebugOutput)
        {
            _cursorDebugLabel = new Label { ZIndex = 2 };

            AbsoluteLayout.SetLayoutFlags(_cursorDebugLabel, AbsoluteLayoutFlags.None);
            AbsoluteLayout.SetLayoutBounds(_cursorDebugLabel, new Rect(0, 300, 100, 300));
            Add(_cursorDebugLabel);
        }

        _menuView = new MenuView();
        _menuView.PopupTarget = this;
        _menuView.CircuitView = _circuitView;
        _menuView.ZIndex = 2;

        AbsoluteLayout.SetLayoutFlags(_menuView, AbsoluteLayoutFlags.WidthProportional);
        AbsoluteLayout.SetLayoutBounds(_menuView, new Rect(0, 0, 1, 36));
        Add(_menuView);

        Loaded += OnLoaded;
        _circuitView.ShowCollisionMap = Convert.ToBoolean(API.GetPreference("ShowTraceCollisionMap"));
        _circuitView.CursorDebugChanged = CursorDebugChanged;
        _circuitView.ItemsView = _itemsView;
    }

    private void CursorDebugChanged()
    {
        if (_showCursorDebugOutput && _cursorDebugLabel != null)
        {
            _cursorDebugLabel.Text = _circuitView.CursorDebugOutput;
        }
    }

    private void OnLoaded(object? sender, EventArgs e)
    {
        _editWindow = new EditView(this);
        _propertiesWindow = new PropertiesView(this) { OnUpdate = OnUpdate };
        _propertiesWindow.PropertyExcludeList.AddRange(
        new[]{
            "IsInsertable", "DefaultValue", "DefaultType", "DrawableComponent", "Pins", "TypeName", "RefName", "ItemGuid"
        });
        _circuitView.OnSelectedItemChange = _propertiesWindow.GetProperties;

        //    BackgroundImageSource = ImageService.BackgroundImageSource(this);
    }

    private async void OnUpdate()
    {
        await _circuitView.Paint();
    }
}
