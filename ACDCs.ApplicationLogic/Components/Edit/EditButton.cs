namespace ACDCs.API.Core.Components.Edit;

using Instance;

public class EditButton : Button
{
    private readonly double _buttonHeight;
    private readonly double _buttonWidth;
    private readonly string _font;
    private readonly bool _isSelectable;
    private readonly Action _onClickAction;
    private readonly Action<EditButton>? _onDeselectAction;
    private readonly Action<EditButton>? _onSelectAction;
    private readonly string _selectedText;
    private readonly string _text;
    private bool _isSelected;

    public EditButton(string text, Action onClickAction, Action<EditButton>? onSelectAction, Action<EditButton>? onDeselectAction, double buttonWidth,
        double buttonHeight, bool isSelectable = false, string selectedText = "", string font = "", int fontSize = 16)
    {
        _text = text;
        _isSelected = false;

        this.Margin(new Thickness(0))
            .Padding(new Thickness(0))
            .CornerRadius(4)
            .WidthRequest(buttonWidth)
            .HeightRequest(buttonHeight)
            .BackgroundColor(Colors.Transparent)
            .Text(text)
            .FontFamily(font)
            .FontSize(fontSize)
            .BorderWidth(0);

        if (!string.IsNullOrEmpty(font))
        {
            _font = font;
        }

        _onClickAction = onClickAction;
        _onSelectAction = onSelectAction;
        _onDeselectAction = onDeselectAction;
        _buttonWidth = buttonWidth;
        _buttonHeight = buttonHeight;
        _isSelectable = isSelectable;
        _selectedText = selectedText;
        Clicked += OnClicked;
        // Source = API.Instance.ButtonImageSource(_text, Convert.ToInt32(_buttonWidth + 20), Convert.ToInt32(_buttonHeight + 20), font);
    }

    public void Deselect()
    {
        _isSelected = false;
        if (!string.IsNullOrEmpty(_selectedText))
        {
            Text = _text;
            //Source = API.Instance.ButtonImageSource(_text, Convert.ToInt32(_buttonWidth + 20), Convert.ToInt32(_buttonHeight + 20), _font);
        }

        this.BackgroundColor(Colors.Transparent);
        _onDeselectAction?.Invoke(this);
    }

    public void Select()
    {
        _isSelected = true;
        if (!string.IsNullOrEmpty(_selectedText))
        {
            Text = _selectedText;
            // Source = API.Instance.ButtonImageSource(_selectedText, Convert.ToInt32(_buttonWidth + 20), Convert.ToInt32(_buttonHeight + 20), _font);
        }
        this.BackgroundColor(API.Instance.Foreground.WithAlpha(0.7f));
        _onSelectAction?.Invoke(this);
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
}
