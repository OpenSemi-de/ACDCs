using System;
using ACDCs.Services;
using Sharp.UI;

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
        this.BackgroundImageSource(ImageService.BackgroundImageSource(this));
    }
}
