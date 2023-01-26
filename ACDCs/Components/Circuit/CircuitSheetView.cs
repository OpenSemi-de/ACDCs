using ACDCs.Components.Menu;
using ACDCs.Components.Menu.MenuHandlers;
using ACDCs.Components.Properties;
using ACDCs.Views.Edit;
using ACDCs.Views.Menu;
using ACDCs.Views.ModelEditor;
using ACDCs.Views.ModelSelection;
using ACDCs.Views.Properties;
using Microsoft.Maui.Layouts;
using AbsoluteLayout = Sharp.UI.AbsoluteLayout;
using ItemsView = ACDCs.Views.Items.ItemsView;
using Label = Sharp.UI.Label;

namespace ACDCs.Components.Circuit;

public sealed class CircuitSheetView : SharpAbsoluteLayout
{
    private readonly CircuitView _circuitView;

    private readonly Label _cursorDebugLabel;
    private readonly ItemsView _itemsView;

    // ReSharper disable once NotAccessedField.Local
    private readonly List<MenuHandler> _menuHandlers;

    private readonly MenuView _menuView;

    // ReSharper disable once NotAccessedField.Local
    private EditView? _editWindow;

    private ModelEditorWindowView? _modelEditorWindow;
    private ModelSelectionWindowView? _modelSelectionWindow;
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

        _circuitView = new CircuitView()
            .PopupTarget(this);
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

        _cursorDebugLabel = new Label() { ZIndex = 2 };

        AbsoluteLayout.SetLayoutFlags(_cursorDebugLabel, AbsoluteLayoutFlags.None);
        AbsoluteLayout.SetLayoutBounds(_cursorDebugLabel, new Rect(0, 300, 100, 300));
        Add(_cursorDebugLabel);

        _menuView = new MenuView()
            .PopupTarget(this)
            .CircuitView(_circuitView);
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
        _cursorDebugLabel.Text = _circuitView.CursorDebugOutput;
    }

    private void OnLoaded(object? sender, EventArgs e)
    {
        _editWindow = new EditView(this);
        _propertiesWindow = new PropertiesView(this) { OnUpdate = OnUpdate };
        _propertiesWindow.PropertyExcludeList.AddRange(
        new[]{
            "IsInsertable", "DefaultValue", "DefaultType", "DrawableComponent", "Pins", "TypeName", "RefName", "ItemGuid"
        });
        _propertiesWindow.OnModelSelectionClicked = OnModelSelectionClicked;
        _circuitView.OnSelectedItemChange = _propertiesWindow.GetProperties;

        _modelSelectionWindow = new ModelSelectionWindowView(this)
        {
            IsVisible = false,
            OnModelSelected = _propertiesWindow.OnModelSelected
        };

        _modelEditorWindow = new ModelEditorWindowView(this)
        {
            IsVisible = false,
            OnModelEdited = _propertiesWindow.OnModelEdited,
        };

        _propertiesWindow.OnModelEditorClicked = OnModelEditorClicked;

        //    BackgroundImageSource = ImageService.BackgroundImageSource(this);
    }

    private void OnModelEditorClicked(PropertyEditorView obj)
    {
        if (_modelEditorWindow != null)
        {
            _modelEditorWindow.IsVisible = true;
            _modelEditorWindow.GetProperties(obj.Value);
        }
    }

    private void OnModelSelectionClicked(PropertyEditorView obj)
    {
        if (_modelSelectionWindow == null)
        {
            return;
        }

        _modelSelectionWindow.SetComponentType(obj.ValueType);
        _modelSelectionWindow.IsVisible = true;
    }

    private void OnUpdate()
    {
        _circuitView?.Paint();
    }
}
