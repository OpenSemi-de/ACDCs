namespace ACDCs.Components.Window;

using Sharp.UI;

public class WindowTab : Frame
{
    private readonly Color _backgroundColor;
    private readonly Action<WindowTab> _callBack;
    private readonly Button _tabButton;

    public WindowTab(string title, Action<WindowTab> callBack)
    {
        this.VerticalOptions(LayoutOptions.Fill)
            .HorizontalOptions(LayoutOptions.Fill)
            .Padding(0)
            .Margin(0)
            .CornerRadius(1);

        _tabButton = new Button(title)
            .VerticalOptions(LayoutOptions.Fill)
            .HorizontalOptions(LayoutOptions.Fill)
            .Padding(new Thickness(3, 0, 3, 0))
            .Margin(0)
            .CornerRadius(1)
            .BackgroundColor(Colors.LightBlue)
            .OnClicked(windowTabClicked);
        Content = _tabButton;
        _backgroundColor = _tabButton.BackgroundColor;
        SetInactive();
        _callBack = callBack;
    }

    public void SetActive()
    {
        _tabButton.BackgroundColor(_backgroundColor.WithAlpha(0.8f));
    }

    public void SetInactive()
    {
        _tabButton.BackgroundColor(_backgroundColor.WithAlpha(0.2f));
    }

    private void windowTabClicked(object? sender, EventArgs e)
    {
        _callBack?.Invoke(this);
    }
}
