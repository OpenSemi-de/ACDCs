namespace ACDCs.API.Windowing.Components.Menu;

using Instance;
using Interfaces;
using Sharp.UI;

public class MenuToggle : StackLayout, IMenuComponent
{
    private readonly Button _button;
    private readonly CheckBox _checkbox;
    public double ItemHeight { get; set; }
    public double ItemWidth { get; set; }

    public string MenuCommand { get; set; }

    public string Text { get; set; }

    public MenuToggle(string text, string menuCommand)
    {
        Text = text;
        MenuCommand = menuCommand;

        this.HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill)
            .Orientation(StackOrientation.Horizontal)
            .BackgroundColor(API.Instance.Foreground);

        _checkbox = new CheckBox();
        _checkbox.CheckedChanged += CheckboxOnCheckedChanged;

        Margin = new Thickness(2, 2, 2, 2);
        ItemHeight = MinimumHeightRequest + Margin.Top + Margin.Bottom;

        _button = new Button()
            .Text(text)
            .TextColor(API.Instance.Text)
            .HorizontalOptions(LayoutOptions.Start)
            .BackgroundColor(Colors.Transparent)
            .BorderColor(Colors.Transparent);

        _button.Clicked += ButtonOnClicked;
        Add(_checkbox);
        Add(_button);
    }

    private void ButtonOnClicked(object? sender, EventArgs e)
    {
        _checkbox.IsChecked = !_checkbox.IsChecked;
    }

    private void CheckboxOnCheckedChanged(object? sender, CheckedChangedEventArgs e)
    {
        API.Call(() =>
        {
            if (MenuCommand != "")
            {
                API.Instance.Call(MenuCommand, _checkbox.IsChecked);
            }

            Task.Delay(200).Wait();
            return Task.CompletedTask;
        }).Wait();
    }
}
