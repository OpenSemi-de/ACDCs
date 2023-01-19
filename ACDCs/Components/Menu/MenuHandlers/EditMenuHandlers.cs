using ACDCs.Services;
using ACDCs.Views.Menu;

namespace ACDCs.Components.Menu.MenuHandlers;

public class EditMenuHandlers : MenuHandlerView
{
    public EditMenuHandlers()
    {
        MenuService.Add("delete", Delete);
        MenuService.Add("duplicate", Duplicate);
        MenuService.Add("mirror", Mirror);
        MenuService.Add("selectall", SelectAll);
        MenuService.Add("deselectall", DeselectAll);
        MenuService.Add("multiselect", SwitchMultiselect);
        MenuService.Add("rotate", Rotate);
    }

    private async void Delete(object? o)
    {
        await EditService.Delete(CircuitView);
    }

    private async void DeselectAll(object? o)
    {
        await EditService.DeselectAll(CircuitView);
    }

    private async void Duplicate(object? o)
    {
        await EditService.Duplicate(CircuitView);
    }

    private async void Mirror(object? o)
    {
        await EditService.Mirror(CircuitView);
    }

    private async void Rotate(object? o)
    {
        await EditService.Rotate(CircuitView);
    }

    private async void SelectAll(object? o)
    {
        await EditService.SelectAll(CircuitView);
    }

    private async void SwitchMultiselect(object? state)
    {
        await EditService.SwitchMultiselect(state, CircuitView);
    }
}
