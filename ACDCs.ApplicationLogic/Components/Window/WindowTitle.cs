namespace ACDCs.ApplicationLogic.Components.Window;

#pragma warning disable IDE0065

using Sharp.UI;

#pragma warning restore IDE0065

public class WindowTitle : Label
{
    public WindowTitle(string windowTitle, Window parentWindow)
    {
        ParentWindow = parentWindow;
        this.Text(windowTitle)
            .HorizontalOptions(LayoutOptions.Fill)
           .VerticalOptions(LayoutOptions.Fill)
           .VerticalTextAlignment(TextAlignment.Center)
           .HorizontalTextAlignment(TextAlignment.Center)
           .Padding(0)
           .Margin(0);
    }

    public Window ParentWindow { get; set; }
}
