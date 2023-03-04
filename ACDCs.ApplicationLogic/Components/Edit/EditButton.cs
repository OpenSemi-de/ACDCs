namespace ACDCs.API.Core.Components.Edit;

using Instance;
using Sharp.UI;

public class EditButton : ImageButton
{
    private readonly double _buttonHeight;
    private readonly double _buttonWidth;
    private readonly bool _isSelectable;
    private readonly Action _onClickAction;
    private readonly Action<EditButton> _onSelectAction;
    private readonly string _text;
    private bool _isSelected;

    public EditButton(string text, Action onClickAction, Action<EditButton> onSelectAction, double buttonWidth,
                                    double buttonHeight, bool isSelectable = false)
    {
        _text = text;
        _isSelected = false;

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
        Source = API.Instance.ButtonImageSource(_text, Convert.ToInt32(_buttonWidth + 20), Convert.ToInt32(_buttonHeight + 20));
    }

    public void Deselect()
    {
        _isSelected = false;
        this.BackgroundColor(Colors.Transparent);
    }

    private async void OnClicked(object? sender, EventArgs e)
    {
        if (_isSelectable)
        {
            if (_isSelected)
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

    private void Select()
    {
        _onSelectAction.Invoke(this);
        _isSelected = true;
        this.BackgroundColor(API.Instance.Foreground.WithAlpha(0.7f));
    }
}
