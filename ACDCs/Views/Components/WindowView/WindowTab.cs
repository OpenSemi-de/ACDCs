using Sharp.UI;
using Button = Sharp.UI.Button;
using Frame = Sharp.UI.Frame;

namespace ACDCs.Views.Components.WindowView;

public class WindowTab : Frame
{
    private readonly Action<WindowTab> _callBack;

    public WindowTab(string title, Action<WindowTab> callBack)
    {
        this.VerticalOptions(LayoutOptions.Fill)
            .HorizontalOptions(LayoutOptions.Fill)
            .Padding(0)
            .Margin(0)
            .CornerRadius(1);

        Content = new Button(title)
            .VerticalOptions(LayoutOptions.Fill)
            .HorizontalOptions(LayoutOptions.Fill)
            .Padding(new Thickness(3, 0, 3, 0))
            .Margin(0)
            .CornerRadius(1)
            .OnClicked(windowTabClicked);

        _callBack = callBack;
    }

    private void windowTabClicked(object? sender, EventArgs e)
    {
        _callBack?.Invoke(this);
    }
}
