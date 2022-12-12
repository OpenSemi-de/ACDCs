using System;
using ACDCs.CircuitRenderer.Items;
using ACDCs.Services;
using Microsoft.Maui.Controls;

namespace ACDCs.Views;

public partial class StartCenterPage : ContentPage
{
    public StartCenterPage()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void Button_OnClicked(object? sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//circuit").Wait();
    }

    private async void CircuitView_OnLoaded(object? sender, EventArgs e)
    {
        TextItem textItemLogo = new("ACDCs", 140, 10, 10);
        TextItem textItemText = new("Advanced Circuit Design Component Suite", 20, 10, 12);
        textItemLogo.IsRealFontSize = true;
        textItemText.IsRealFontSize = true;
        textItemLogo.Width = 10;
        textItemLogo.Height = 6;
        CircuitView.InsertToPosition(10, 10, textItemLogo).Wait();
        CircuitView.InsertToPosition(10, 12, textItemText).Wait();
        await CircuitView.Paint();
    }

    private void OnLoaded(object? sender, EventArgs e)
    {
        BackgroundImageSource = ImageService.BackgroundImageSource(this);
    }
}
