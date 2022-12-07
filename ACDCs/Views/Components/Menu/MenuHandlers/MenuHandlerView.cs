using ACDCs.Views.Components.CircuitView;
using ACDCs.Views.Components.DebugView;
using Microsoft.Maui.Controls;

namespace ACDCs.Views.Components.Menu.MenuHandlers;

public class MenuHandlerView : ContentView
{
    private static readonly BindableProperty CircuitViewProperty =
        BindableProperty.Create(nameof(CircuitView), typeof(CircuitViewContainer), typeof(CircuitSheetPage));

    private static readonly BindableProperty PopupPageProperty =
            BindableProperty.Create(nameof(Page), typeof(Page), typeof(CircuitSheetPage));

    private static readonly BindableProperty DebugViewProperty =
        BindableProperty.Create(nameof(DebugView), typeof(DebugViewDragComtainer), typeof(CircuitSheetPage));


    public CircuitViewContainer CircuitView
    {
        get => (CircuitViewContainer)GetValue(CircuitViewProperty);
        set => SetValue(CircuitViewProperty, value);
    }

    public Page PopupPage
    {
        get => (Page)GetValue(PopupPageProperty);
        set => SetValue(PopupPageProperty, value);
    }

    public DebugViewDragComtainer DebugView
    {
        get => (DebugViewDragComtainer)GetValue(DebugViewProperty);
        set => SetValue(DebugViewProperty, value);
    }
}
