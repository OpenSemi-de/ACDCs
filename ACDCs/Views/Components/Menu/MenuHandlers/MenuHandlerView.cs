using ACDCs.Views.Components.CircuitView;
using ACDCs.Views.Components.DebugView;

namespace ACDCs.Views.Components.Menu.MenuHandlers;

using Sharp.UI;

[BindableProperties]
public interface IMenuHandlerView
{
    CircuitViewContainer CircuitView { get; set; }
    ComponentsPage ComponentsPage { get; set; }
    DebugViewDragComtainer DebugView { get; set; }
    Page PopupPage { get; set; }
}

[SharpObject]
public partial class MenuHandlerView : ContentView, IMenuHandlerView
{
}
