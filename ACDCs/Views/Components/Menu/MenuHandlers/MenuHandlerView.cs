using ACDCs.Views.Components.CircuitView;

namespace ACDCs.Views.Components.Menu.MenuHandlers;

using Sharp.UI;

[BindableProperties]
public interface IMenuHandlerView
{
    CircuitViewContainer CircuitView { get; set; }
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
