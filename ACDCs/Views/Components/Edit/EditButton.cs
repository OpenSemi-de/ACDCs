using ACDCs.Services;

namespace ACDCs.Views.Components.Edit;

using Sharp.UI;

public class EditButton : ImageButton
{
    private readonly double _buttonHeight;

    private readonly double _buttonWidth;

    private readonly bool _isSelectable;

    private readonly Action _onClickAction;

    private readonly Action<EditButton> _onSelectAction;

    public bool IsSelected { get; set; }

    public string Text { get; set; }

    public EditButton(string text, Action onClickAction, Action<EditButton> onSelectAction, double buttonWidth,
                                    double buttonHeight, bool isSelectable = false)
    {
        Text = text;
        IsSelected = false;

        this.Margin(new Thickness(0))
            .Padding(new Thickness(0))
            .CornerRadius(4)
            .WidthRequest(buttonWidth)
            .HeightRequest(buttonHeight)
            .BackgroundColor(Colors.Transparent)
            .BorderWidth(0)
            .Aspect(Aspect.Fill);

        _onClickAction = onClickAction;
        _onSelectAction = onSelectAction;
        _buttonWidth = buttonWidth;
        _buttonHeight = buttonHeight;
        _isSelectable = isSelectable;
        Clicked += OnClicked;
        Source = ImageService.ButtonImageSource(Text, Convert.ToInt32(_buttonWidth + 20), Convert.ToInt32(_buttonHeight + 20));
    }

    public void Deselect()
    {
        IsSelected = false;
        this.BackgroundColor(Colors.Transparent);
    }

    public void Select()
    {
        _onSelectAction.Invoke(this);
        IsSelected = true;
        this.BackgroundColor(ColorService.Foreground.WithAlpha(0.7f));
    }

    private async void OnClicked(object? sender, EventArgs e)
    {
        if (_isSelectable)
        {
            if (IsSelected)
            {
                Deselect();
            }
            else
            {
                Select();
            }
        }
        else
        {
            Select();

            _onClickAction.Invoke();
            await Task.Delay(200);
            Deselect();
        }
    }
}
