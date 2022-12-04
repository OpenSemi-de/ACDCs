using Microsoft.Maui.Controls;

namespace ACDCs.Views.Components.Menu.MenuHandlers;

public class MenuHandlerView : ContentView
{
    private static readonly BindableProperty CircuitViewProperty =
        BindableProperty.Create(nameof(CircuitView), typeof(CircuitView.CircuitView), typeof(CircuitSheetPage));

    public CircuitView.CircuitView CircuitView
    {
        get => (CircuitView.CircuitView)GetValue(CircuitViewProperty);
        set => SetValue(CircuitViewProperty, value);
    }
}