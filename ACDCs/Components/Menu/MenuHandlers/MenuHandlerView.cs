using ACDCs.Views;
using ACDCs.Views.Circuit;

namespace ACDCs.Components.Menu.MenuHandlers;

using Sharp.UI;

[BindableProperties]
public interface IMenuHandlerView
{
    CircuitView CircuitView { get; set; }
    ComponentsView ComponentsView { get; set; }
    Page PopupPage { get; set; }
}

[SharpObject]
public partial class MenuHandlerView : ContentView, IMenuHandlerView
{
    public MenuHandlerView()
    {
    }
}
