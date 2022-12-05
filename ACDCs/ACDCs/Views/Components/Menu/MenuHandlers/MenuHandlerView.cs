using Microsoft.Maui.Controls;

namespace ACDCs.Views.Components.Menu.MenuHandlers;

public class MenuHandlerView : ContentView
{
    private static readonly BindableProperty CircuitViewProperty =
        BindableProperty.Create(nameof(CircuitView), typeof(CircuitView.CircuitView), typeof(CircuitSheetPage));
    private static readonly BindableProperty PopupPageProperty =
        BindableProperty.Create(nameof(Page), typeof(Page), typeof(CircuitSheetPage));

    public CircuitView.CircuitView CircuitView
    {
        get => (CircuitView.CircuitView)GetValue(CircuitViewProperty);
        set => SetValue(CircuitViewProperty, value);
    }
    public Page PopupPage
    {
        get => (Page)GetValue(PopupPageProperty);
        set => SetValue(PopupPageProperty, value);
    }
}