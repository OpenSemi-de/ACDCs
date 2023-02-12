namespace ACDCs.ApplicationLogic.Components.QuickEdit;

using Window;

// ReSharper disable once UnusedType.Global
public class QuickEditWindow : Window
{
    public QuickEditWindow(WindowContainer? container) : base(container, "QuickEdit", "", false, GetView)
    {
        Start();
        container?.SetWindowPosition(this, 400, 4);
        container?.SetWindowSize(this, 400, 90);
    }

    private static View GetView(Window arg)
    {
        return new QuickEditView();
    }
}
