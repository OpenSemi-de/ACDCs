using ACDCs.Services;


namespace ACDCs.Views.Components.Edit;

using Sharp.UI;
public class EditButton : ImageButton
{

    public EditButton(string text, Action onClickAction, Action<EditButton> onSelectAction, double buttonWidth, double buttonHeight)
    {
        Text = text;
        IsSelected = false;

        this.Margin(new Thickness(0))
            .Padding(new Thickness(2))
            .CornerRadius(4)
            .WidthRequest(buttonWidth)
            .HeightRequest(buttonHeight)
            .BackgroundColor(Colors.Transparent)
            .BorderWidth(0);

        this.Shadow(new Shadow());

        _onClickAction = onClickAction;
        _onSelectAction = onSelectAction;
        _buttonWidth = buttonWidth;
        _buttonHeight = buttonHeight;
        Clicked += OnClicked;
        Loaded += OnLoaded;
    }



    public bool IsSelected { get; set; }

    private void OnLoaded(object? sender, EventArgs e)
    {
        Source = ImageService.ButtonImageSource(Text, (int)_buttonWidth, (int)_buttonHeight);
    }

    public string Text { get; set; }

    private readonly Action _onClickAction;
    private readonly Action<EditButton> _onSelectAction;
    private readonly double _buttonWidth;
    private readonly double _buttonHeight;


    private void OnClicked(object? sender, EventArgs e)
    {
        if (IsSelected)
        {
            Deselect();
        }
        else
        {
            Select();
        }

        _onClickAction.Invoke();
    }

    public void Select()
    {
        _onSelectAction.Invoke(this);
        IsSelected = true;
        this.BackgroundColor(Colors.White.WithAlpha(0.7f));
    }

    public void Deselect()
    {
        IsSelected = false;
        this.BackgroundColor(Colors.Transparent);
    }
}
