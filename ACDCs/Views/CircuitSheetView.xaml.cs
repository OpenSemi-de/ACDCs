using ACDCs.Components;
using ACDCs.Views.ModelEditor;
using ACDCs.Views.ModelSelection;
using ACDCs.Views.Properties;
using EditView = ACDCs.Views.Edit.EditView;
using PropertyEditorView = ACDCs.Views.Properties.PropertyEditorView;

namespace ACDCs.Views;

public partial class CircuitSheetView : SharpAbsoluteLayout
{
    // ReSharper disable once NotAccessedField.Local
    private EditView? _editWindow;

    private ModelEditorWindowView? _modelEditorWindow;
    private ModelSelectionWindowView? _modelSelectionWindow;
    private PropertiesView? _propertiesWindow;

    public CircuitSheetView()
    {
        InitializeComponent();
        Loaded += OnLoaded;
        CircuitView.CursorDebugChanged = CursorDebugChanged;
        CircuitView.ItemsView = ItemsView;
    }

    private void CursorDebugChanged()
    {
        CursorDebugLabel.Text = CircuitView.CursorDebugOutput;
    }

    private void OnLoaded(object? sender, EventArgs e)
    {
        _editWindow = new EditView(AbsoluteLayoutSheetPage);
        _propertiesWindow = new PropertiesView(AbsoluteLayoutSheetPage) { OnUpdate = OnUpdate };
        _propertiesWindow.PropertyExcludeList.AddRange(
        new[]{
            "IsInsertable", "DefaultValue", "DefaultType", "DrawableComponent", "Pins", "TypeName", "RefName", "ItemGuid"
        });
        _propertiesWindow.OnModelSelectionClicked = OnModelSelectionClicked;
        CircuitView.OnSelectedItemChange = _propertiesWindow.GetProperties;

        _modelSelectionWindow = new ModelSelectionWindowView(AbsoluteLayoutSheetPage)
        {
            IsVisible = false,
            OnModelSelected = _propertiesWindow.OnModelSelected
        };

        _modelEditorWindow = new ModelEditorWindowView(AbsoluteLayoutSheetPage)
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
        CircuitView?.Paint();
    }
}
