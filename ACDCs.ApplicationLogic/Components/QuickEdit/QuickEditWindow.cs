namespace ACDCs.ApplicationLogic.Components.QuickEdit;

using Window;

// ReSharper disable once UnusedType.Global
public class QuickEditWindow : Window
{
    public QuickEditView EditView { get; set; }

    public QuickEditWindow(WindowContainer? container) : base(container, "QuickEdit", "", false, GetView, titleHeight: 28)
    {
        Start();
        HideResizer();
        HideWindowButtons();
        container?.SetWindowPosition(this, 260, 4);
        container?.SetWindowSize(this, 400, 90);
    }

    private static View GetView(Window window)
    {
        QuickEditView quickEditView = new QuickEditView();
        if (window is QuickEditWindow quickEditWindow)
        {
            quickEditWindow.EditView = quickEditView;
        }

        return quickEditView;
    }
}
