using Microsoft.Maui.Accessibility;
using Microsoft.Maui.Controls;
using System;

namespace ACDCs.Views;

public partial class StartCenterPage : ContentPage
{
    private int count = 0;

    public StartCenterPage()
    {
        InitializeComponent();
    }

    private void OnCounterClicked(object sender, EventArgs e)
    {
        count++;

        if (count == 1)
            CounterBtn.Text = $"Clicked {count} time";
        else
            CounterBtn.Text = $"Clicked {count} times";

        SemanticScreenReader.Announce(CounterBtn.Text);
    }
}