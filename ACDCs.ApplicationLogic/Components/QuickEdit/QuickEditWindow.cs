namespace ACDCs.API.Core.Components.QuickEdit;

using Windowing.Components.Window;

// ReSharper disable once UnusedType.Global
public class QuickEditWindow : Window
{
    private readonly Action? _onUpdate;
    public QuickEditView? EditView { get; set; }

    public QuickEditWindow(WindowContainer? container, Action? onUpdate) : base(container, "QuickEdit", "", false, GetView, titleHeight: 28)
    {
        _onUpdate = onUpdate;
        Start();
        HideResizer();
        HideWindowButtons();
        QuickEditView? quickEditView = this.EditView;
        if (quickEditView != null)
        {
            quickEditView.OnUpdatedValue = _onUpdate;
            quickEditView.ParentContainer = container;
            quickEditView.Initialize();
        }

        container?.SetWindowPosition(this, 260, 4);
        container?.SetWindowSize(this, 400, 70);
    }

    private static View GetView(Window window)
    {
        QuickEditView quickEditView = new(window);
        if (window is QuickEditWindow quickEditWindow)
        {
            quickEditWindow.EditView = quickEditView;
        }

        return quickEditView;
    }
}
