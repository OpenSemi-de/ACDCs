using Sharp.UI;
using ContentView = Sharp.UI.ContentView;
using Label = Sharp.UI.Label;

namespace ACDCs.Components.Window;

public class WindowTitleLabel : ContentView
{
    private readonly Label _label;

    public WindowTitleLabel(string windowTitle)
    {
        _label = new Label(windowTitle)
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill)
            .VerticalTextAlignment(TextAlignment.Center)
            .HorizontalTextAlignment(TextAlignment.Center)
            .FontSize(14)
            .FontAttributes(FontAttributes.Bold)
            .Padding(0)
            .Margin(0);

        this.Padding(0).Margin(1)
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill);

        Content = _label;
    }

    public WindowTitleLabel TextColor(Color color)
    {
        _label.TextColor(color);
        return this;
    }
}
