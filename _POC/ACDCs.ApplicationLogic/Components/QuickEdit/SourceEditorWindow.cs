namespace ACDCs.API.Core.Components.QuickEdit;

using ACDCs.CircuitRenderer.Items;
using CircuitRenderer.Items.Sources;
using Interfaces;
using Windowing.Components.Window;

public class SourceEditorWindow : Window, IWindow
{
    private SourceEditorView? _sourceEditView;

    public Action<WorksheetItem>? OnSourceEdited
    {
        get => SourceEditView?.OnSourceEdited;
        set
        {
            if (SourceEditView != null)
            {
                SourceEditView.OnSourceEdited = value;
            }
        }
    }

    private SourceEditorView? SourceEditView
    {
        get => _sourceEditView;
        set => _sourceEditView = value;
    }

    public SourceEditorWindow(WindowContainer? container) : base(container, "Edit source", "", childViewFunction: GetView)
    {
        Start();
    }

    public void SetSource(VoltageSourceItem source)
    {
        _sourceEditView?.SetSource(source);
    }

    private static View? GetView(Window obj)
    {
        if (obj is not SourceEditorWindow window) return new Label("Error");
        window.SourceEditView = new SourceEditorView();
        return window.SourceEditView;
    }
}
