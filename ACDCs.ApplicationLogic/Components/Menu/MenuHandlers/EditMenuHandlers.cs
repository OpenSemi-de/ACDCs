using ACDCs.ApplicationLogic.Components.Circuit;

namespace ACDCs.ApplicationLogic.Components.Menu.MenuHandlers;

public class EditMenuHandlers : MenuHandler
{
    public CircuitView? CircuitView
    {
        get => GetParameter<CircuitView>("CircuitView");
    }

    public EditMenuHandlers()
    {
        API.Instance.Add("delete", Delete);
        API.Instance.Add("duplicate", Duplicate);
        API.Instance.Add("mirror", Mirror);
        API.Instance.Add("selectall", SelectAll);
        API.Instance.Add("deselectall", DeselectAll);
        API.Instance.Add("multiselect", SwitchMultiselect);
        API.Instance.Add("rotate", Rotate);
    }

    private async void Delete(object? o)
    {
        if (CircuitView != null)
        {
            await API.Instance.Delete(CircuitView);
        }
    }

    private async void DeselectAll(object? o)
    {
        if (CircuitView != null)
        {
            await API.Instance.DeselectAll(CircuitView);
        }
    }

    private async void Duplicate(object? o)
    {
        if (CircuitView != null)
        {
            await API.Instance.Duplicate(CircuitView);
        }
    }

    private async void Mirror(object? o)
    {
        if (CircuitView != null)
        {
            await API.Instance.Mirror(CircuitView);
        }
    }

    private async void Rotate(object? o)
    {
        if (CircuitView != null)
        {
            await API.Instance.Rotate(CircuitView);
        }
    }

    private async void SelectAll(object? o)
    {
        if (CircuitView != null)
        {
            await API.Instance.SelectAll(CircuitView);
        }
    }

    private async void SwitchMultiselect(object? state)
    {
        if (CircuitView != null)
        {
            await API.Instance.SwitchMultiselect(state, CircuitView);
        }
    }
}
