namespace ACDCs.API.Core.Components.QuickEdit;

using ACDCs.API.Windowing.Components.Window;

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
        container?.SetWindowSize(this, 400, 70);
    }

    private static View GetView(Window window)
    {
        QuickEditView quickEditView = new QuickEditView(window);
        if (window is QuickEditWindow quickEditWindow)
        {
            quickEditWindow.EditView = quickEditView;
        }

        return quickEditView;
    }
}
