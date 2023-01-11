using ACDCs.Views.Components;

namespace ACDCs.Views;

public partial class CircuitSheetView : SharpAbsoluteLayout
{
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
        //    BackgroundImageSource = ImageService.BackgroundImageSource(this);
    }
}
