using ACDCs.Views.Components;
using ACDCs.Views.Components.Edit;

namespace ACDCs.Views;

public partial class CircuitSheetView : SharpAbsoluteLayout
{
    private EditView _editWindow;
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
        CircuitView.OnSelectedItemChange = _propertiesWindow.GetProperties;

        //    BackgroundImageSource = ImageService.BackgroundImageSource(this);
    }
}
