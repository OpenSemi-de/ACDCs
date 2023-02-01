namespace ACDCs.ApplicationLogic.Components.Window;

#pragma warning disable IDE0065

using Sharp.UI;

#pragma warning restore IDE0065

[SharpObject]
public partial class WindowTitleLabel : ContentView, IWindowTitleLabelProperties
{
    private readonly Label _label;

    public WindowTitleLabel(string windowTitle)
    {
        _label = LabelGeneratedExtension.FontFamily(new Label(windowTitle), (string)FontFamily)
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

[BindableProperties]
public interface IWindowTitleLabelProperties
{
    string FontFamily { get; set; }
}
