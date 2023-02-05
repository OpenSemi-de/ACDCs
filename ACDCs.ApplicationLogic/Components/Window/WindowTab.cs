namespace ACDCs.ApplicationLogic.Components.Window;

using Sharp.UI;

public class WindowTab : Frame
{
    private readonly Action<WindowTab>? _callBack;
    private readonly Button _tabButton;

    public WindowTab(string title, Action<WindowTab>? callBack)
    {
        this.VerticalOptions(LayoutOptions.Fill)
            .HorizontalOptions(LayoutOptions.Fill)
            .Padding(0)
            .Margin(0)
            .CornerRadius(1);

        _tabButton = new Button(title)
            .VerticalOptions(LayoutOptions.Fill)
            .HorizontalOptions(LayoutOptions.Fill)
            .Padding(new Thickness(6, 0, 6, 0))
            .Margin(0)
            .CornerRadius(1)
            .BackgroundColor(Colors.LightBlue)
            .OnClicked(WindowTabClicked);

        _tabButton.BackgroundColor = API.Instance.Foreground;
        _tabButton.TextColor = API.Instance.Text;
        _tabButton.BorderColor = Colors.Transparent;

        Content = _tabButton;

        SetInactive();
        _callBack = callBack;
    }

    public void SetActive()
    {
        _tabButton.BackgroundColor(API.Instance.Foreground);
        _tabButton.TextColor(API.Instance.Text);
        _tabButton.BorderColor(API.Instance.Border);
    }

    public void SetInactive()
    {
        _tabButton.BackgroundColor(API.Instance.Background);
        _tabButton.TextColor(API.Instance.Foreground);
        _tabButton.BorderColor(Colors.Transparent);
    }

    private void WindowTabClicked(object? sender, EventArgs e)
    {
        _callBack?.Invoke(this);
    }
}
