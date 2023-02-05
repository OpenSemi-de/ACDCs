namespace ACDCs.ApplicationLogic.Components.Window;

using Sharp.UI;

public class WindowTitle : Label
{
    public Window ParentWindow { get; set; }

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
}
