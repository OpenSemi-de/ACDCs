namespace ACDCs.ApplicationLogic.Components.ModelEditor;

using Data.ACDCs.Interfaces;
using Window;

public class ModelEditorWindow : Window
{
    public Action<IElectronicComponent>? OnModelEdited
    {
        set
        {
            if (ModelEditorView != null)
            {
                ModelEditorView.OnModelEdited = value;
            }
        }
    }

    private ModelEditorView? ModelEditorView
    {
        get => CurrentView as ModelEditorView;
    }

    public ModelEditorWindow(WindowContainer? layout) : base(layout, "Edit model",
                childViewFunction: GetView)
    {
        Start();
    }

    public void GetProperties(IElectronicComponent component)
    {
        ModelEditorView?.GetProperties(component);
    }

    private static View GetView(Window window)
    {
        var modelEditor = new ModelEditorView(window);
        return modelEditor;
    }
}
