using ACDCs.ApplicationLogic.Views.Edit;
using ACDCs.ApplicationLogic.Views.Properties;
using Microsoft.Maui.Layouts;
using ItemsView = ACDCs.ApplicationLogic.Views.Items.ItemsView;

namespace ACDCs.ApplicationLogic.Components.Circuit;

using Window;

#pragma warning disable IDE0065

using Sharp.UI;

#pragma warning restore IDE0065

public sealed class CircuitSheetView : AbsoluteLayout
{
    private readonly WindowContainer _container;

    private readonly Label? _cursorDebugLabel;
    private readonly ItemsView _itemsView;
    private readonly bool _showCursorDebugOutput = Convert.ToBoolean(API.GetPreference("ShowDebugCursorOutput"));

    // ReSharper disable once NotAccessedField.Local
    private EditView? _editWindow;

    private PropertiesWindow? _propertiesWindow;

    public CircuitSheetView(WindowContainer container)
    {
        _container = container;
        _itemsView = new ItemsView
        {
            ButtonWidth = 60,
            ButtonHeight = 60,
            ZIndex = 10
        };

        AbsoluteLayout.SetLayoutFlags(_itemsView, AbsoluteLayoutFlags.WidthProportional | AbsoluteLayoutFlags.YProportional);
        AbsoluteLayout.SetLayoutBounds(_itemsView, new Rect(0, 1, 1, 60));
        Add(_itemsView);

        CircuitView = new CircuitView { PopupTarget = this, ZIndex = 0 };

        AbsoluteLayout.SetLayoutFlags(CircuitView, AbsoluteLayoutFlags.SizeProportional);
        AbsoluteLayout.SetLayoutBounds(CircuitView, new Rect(0, 0, 1, 1));
        Add(CircuitView);

        if (_showCursorDebugOutput)
        {
            _cursorDebugLabel = new Label { ZIndex = 2 };

            AbsoluteLayout.SetLayoutFlags(_cursorDebugLabel, AbsoluteLayoutFlags.None);
            AbsoluteLayout.SetLayoutBounds(_cursorDebugLabel, new Rect(0, 300, 100, 300));
            Add(_cursorDebugLabel);
        }

        Loaded += OnLoaded;
        CircuitView.ShowCollisionMap = Convert.ToBoolean(API.GetPreference("ShowTraceCollisionMap"));
        CircuitView.CursorDebugChanged = CursorDebugChanged;
        CircuitView.ItemsView = _itemsView;
    }

    public CircuitView CircuitView { get; }

    private void CursorDebugChanged()
    {
        if (_showCursorDebugOutput && _cursorDebugLabel != null)
        {
            _cursorDebugLabel.Text = CircuitView.CursorDebugOutput;
        }
    }

    private void OnLoaded(object? sender, EventArgs e)
    {
        _editWindow = new EditView(_container);
        _propertiesWindow = new PropertiesWindow(_container) { OnUpdate = OnUpdate };
        _propertiesWindow.PropertyExcludeList.AddRange(
        new[]{
            "IsInsertable", "DefaultValue", "DefaultType", "DrawableComponent", "Pins", "TypeName", "RefName", "ItemGuid"
        });
        CircuitView.OnSelectedItemChange = _propertiesWindow.GetProperties;
    }

    private async void OnUpdate()
    {
        await CircuitView.Paint();
    }
}
