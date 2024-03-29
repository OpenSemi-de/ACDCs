namespace ACDCs.API.Core.Components.Circuit;

using ACDCs.API.Core.Components.Simulation;
using CircuitRenderer.Interfaces;
using Edit;
using Instance;
using Items;
using Microsoft.Maui.Layouts;
using Properties;
using QuickEdit;
using Windowing.Components.Window;

public sealed class CircuitSheetView : AbsoluteLayout
{
    private readonly WindowContainer _container;
    private readonly Label? _cursorDebugLabel;
    private readonly ItemsView _itemsView;
    private readonly bool _showCursorDebugOutput = Convert.ToBoolean(API.GetPreference("ShowDebugCursorOutput"));

    // ReSharper disable once NotAccessedField.Local
#pragma warning disable IDE0052
    private EditWindow? _editWindow;
#pragma warning restore IDE0052

    private PropertiesWindow? _propertiesWindow;
    private QuickEditWindow? _quickEditWindow;
    private SimulationController? _simulationController;
    private SimulationControlWindow? _simulationControlWindow;
    private SimulationGraphWindow _simulationGraphWindow;
    private SimulationLogWindow _simulationLogWindow;
    public CircuitView CircuitView { get; }

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

        CircuitView = new CircuitView { ZIndex = 0 };

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
        CircuitView.OnSelectedItemChange = OnSelectedItemChange;
    }

    private void CursorDebugChanged()
    {
        if (_showCursorDebugOutput && _cursorDebugLabel != null)
        {
            _cursorDebugLabel.Text = CircuitView.CursorDebugOutput;
        }
    }

    private void OnLoaded(object? sender, EventArgs e)
    {
        _editWindow = new EditWindow(_container);

        _quickEditWindow = new QuickEditWindow(_container, OnUpdate);

        _propertiesWindow = new PropertiesWindow(_container) { OnUpdate = OnUpdate };
        _propertiesWindow.PropertyExcludeList.AddRange(
      new[]{
              "IsInsertable", "DefaultValue", "DefaultType", "DrawableComponent", "Pins", "TypeName", "RefName", "ItemGuid"
        });
        _propertiesWindow.IsVisible = false;
        _propertiesWindow.FadeTo(0);
        CircuitView.CallPropertiesShow = ShowProperties;

        _simulationController = new SimulationController();
        _simulationControlWindow = new SimulationControlWindow(_container);
        _simulationControlWindow.SetSimulation(_simulationController);
        _simulationController.Sheet = CircuitView.CurrentWorksheet;
        _simulationController.GetSheet = CircuitView.GetSheet;
        _simulationLogWindow = new SimulationLogWindow(_container);
        _simulationControlWindow.LogWindow = _simulationLogWindow;
        _simulationController.LogMethod = _simulationLogWindow.AddLog;
        _simulationControlWindow.OnLogVisibilityChanged = _simulationLogWindow.VisibilityChanged;
        _simulationGraphWindow = new SimulationGraphWindow(_container);
        _simulationControlWindow.OnGraphVisibilityChanged = _simulationGraphWindow.VisibilityChanged;
        _simulationLogWindow.IsVisible = false;
        _simulationGraphWindow.IsVisible = false;
    }

    private void OnSelectedItemChange(IWorksheetItem obj)
    {
        _quickEditWindow?.EditView?.UpdateEditor(obj);
        _simulationControlWindow?.SelectItem(obj);
    }

    private async void OnUpdate()
    {
        await CircuitView.Paint();
    }

    private void ShowProperties(IWorksheetItem? obj)
    {
        if (obj == null || _propertiesWindow == null)
        {
            return;
        }

        _propertiesWindow.GetProperties(obj);
        _propertiesWindow.IsVisible = true;
        _propertiesWindow.FadeTo(1);
    }
}
