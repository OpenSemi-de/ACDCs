using ACDCs.Services;

namespace ACDCs.Views;

public partial class CircuitSheetPage : ContentPage
{
    public CircuitSheetPage()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object? sender, EventArgs e)
    {
        BackgroundImageSource = ImageService.BackgroundImageSource(this);
    }
}
