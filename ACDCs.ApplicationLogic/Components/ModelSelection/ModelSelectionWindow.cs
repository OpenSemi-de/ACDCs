namespace ACDCs.API.Core.Components.ModelSelection;

using ACDCs.API.Windowing.Components.Window;
using ACDCs.Data.ACDCs.Interfaces;
using Sharp.UI;

public class ModelSelectionWindow : Window, Interfaces.IWindow
{
    public ModelSelectionView? ModelSelectionView { get; set; }

    public Action<IElectronicComponent>? OnModelSelected { get; set; }

    public ModelSelectionWindow(WindowContainer? container) : base(container, "Select model", "", false, GetView)
    {
        Start();
    }

    public void SetComponentType(string name)
    {
        ModelSelectionView?.SetComponentType(name);
    }

    private static View GetView(Window window)
    {
        if (window is not ModelSelectionWindow modelSelectionWindow)
        {
            return new Label("error");
        }

        ModelSelectionView modelSelectionView = new(window) { OnModelSelected = modelSelectionWindow.OnModelSelected };
        modelSelectionWindow.ModelSelectionView = modelSelectionView;
        modelSelectionWindow.ModelSelectionView.OnModelSelected = component => modelSelectionWindow.OnModelSelected?.Invoke(component);
        return modelSelectionView;
    }
}
