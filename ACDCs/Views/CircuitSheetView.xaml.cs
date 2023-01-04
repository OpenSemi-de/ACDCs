using ACDCs.Views.Components;

namespace ACDCs.Views;

public partial class CircuitSheetView : SharpAbsoluteLayout
{
    public CircuitSheetView()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object? sender, EventArgs e)
    {
        //    BackgroundImageSource = ImageService.BackgroundImageSource(this);
    }
}
