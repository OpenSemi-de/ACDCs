using ACDCs.Views.Components;
using ACDCs.Views.Components.Edit;
using ACDCs.Views.Components.ModelSelection;

namespace ACDCs.Views;

public partial class CircuitSheetView : SharpAbsoluteLayout
{
    private EditView _editWindow;
    private ModelSelectionWindowView _modelSelectionWindow;
    private PropertiesView _propertiesWindow;

    public CircuitSheetView()
    {
        InitializeComponent();
        Loaded += OnLoaded;
        CircuitView.CursorDebugChanged = CursorDebugChanged;
    }

    private void CursorDebugChanged()
    {
        CursorDebugLabel.Text = CircuitView.CursorDebugOutput;
    }

    private void OnLoaded(object? sender, EventArgs e)
    {
        _editWindow = new EditView(AbsoluteLayoutSheetPage);
        _propertiesWindow = new PropertiesView(AbsoluteLayoutSheetPage);
        _propertiesWindow.OnUpdate = OnUpdate;
        _propertiesWindow.PropertyExcludeList.AddRange(
        new[]{
            "IsInsertable", "DefaultValue", "DefaultType", "DrawableComponent", "Pins"
        });
        _propertiesWindow.OnModelSelectionClicked = OnModelSelectionClicked;
        CircuitView.OnSelectedItemChange = _propertiesWindow.GetProperties;

        _modelSelectionWindow = new ModelSelectionWindowView(AbsoluteLayoutSheetPage);
        _modelSelectionWindow.IsVisible = false;
        _modelSelectionWindow.OnModelSelected = _propertiesWindow.OnModelSelected;

        //    BackgroundImageSource = ImageService.BackgroundImageSource(this);
    }

    private void OnModelSelectionClicked(PropertyEditor obj)
    {
        _modelSelectionWindow.SetComponentType(obj.ValueType);
        _modelSelectionWindow.IsVisible = true;
    }

    private void OnUpdate()
    {
        CircuitView?.Paint();
    }
}
