using ACDCs.Views.Components.Menu.MenuHandlers;

namespace ACDCs.Views.Components.Menu;

using Sharp.UI;

public class MenuToggle : StackLayout, IMenuItem
{
    private readonly CheckBox _checkbox;
    private readonly Button _button;

    public MenuToggle(string text, string menuCommand)
    {

        Text = text;
        MenuCommand = menuCommand;

        this.HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill)
            .Orientation(StackOrientation.Horizontal);

        _checkbox = new CheckBox();
        _checkbox.CheckedChanged += CheckboxOnCheckedChanged;


            Margin = new(2, 2, 2, 2);
        ItemHeight = MinimumHeightRequest + Margin.Top + Margin.Bottom;

        _button = new Button()
            .Text(text)
            .HorizontalOptions(LayoutOptions.Start)
            .BackgroundColor(Colors.Transparent)
            .BorderColor(Colors.Transparent);

        _button.Clicked += ButtonOnClicked;
        Add(_checkbox);
        Add(_button);

    }

    private void CheckboxOnCheckedChanged(object? sender, CheckedChangedEventArgs e)
    {
        App.Call(() =>
        {

            if (MenuCommand != "")
            {
                MenuHandler.Call(MenuCommand, _checkbox.IsChecked);
            }

            Task.Delay(200).Wait();
            return Task.CompletedTask;
        }).Wait();
    }

    private void ButtonOnClicked(object? sender, EventArgs e)
    {
        _checkbox.IsChecked = !_checkbox.IsChecked;
    }

    public double ItemHeight { get; set; }
    public string MenuCommand { get; set; }
    public string Text { get; set; }
}
