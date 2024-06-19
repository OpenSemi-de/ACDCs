namespace ACDCs.API.Core.Components.Edit;

using Windowing.Components.Window;

public class EditWindow : Window
{
    public EditWindow(WindowContainer? layout) : base(layout, "Tools", "", false, GetView, titleHeight: 20)
    {
        Start();
        layout?.SetWindowPosition(this, 4, 50);
        HideWindowButtons();
        HideResizer();
        layout?.SetWindowSize(this, 104, 316);
    }

    private static View GetView(Window arg)
    {
        return new EditView();
    }
}
