namespace ACDCs.ApplicationLogic.Components.Menu;

using Interfaces;
using Sharp.UI;

public class MenuDivider : ContentView, IMenuComponent
{
    public double ItemHeight { get; set; } = 4;
    public double ItemWidth { get; set; } = 80;
    public string MenuCommand { get; set; } = string.Empty;

    // ReSharper disable once UnusedMember.Global
    public List<IMenuComponent> MenuItems { get; set; } = new();

    public string Text { get; set; } = string.Empty;

    public MenuDivider()
    {
        Line line = new(0, 0, 80, 0)
        {
            X1 = 1,
            Y1 = 1,
            X2 = 80,
            Y2 = 1,
            Aspect = Stretch.Fill,
            StrokeThickness = 3,
            Stroke = new SolidColorBrush(API.Instance.Text)
        };
        BackgroundColor = API.Instance.Text;
        HeightRequest = 4;
        HorizontalOptions = LayoutOptions.Fill;
        MaximumHeightRequest = 2;
        Margin = 0;
        Padding = 0;
        Content = line;
    }
}
