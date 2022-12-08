using System;
using Microsoft.Maui.Accessibility;
using Microsoft.Maui.Controls;

namespace ACDCs.Views;

public partial class StartCenterPage : ContentPage
{
    public StartCenterPage()
    {
        InitializeComponent();
    }

    private int count = 0;

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
