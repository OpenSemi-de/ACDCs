namespace ACDCs.ApplicationLogic.Components.Edit;

using Sharp.UI;
using Window;

public class EditWindow : Window
{
    private readonly WindowContainer? _layout;
    private EditButton? _deleteButton;
    private EditButton? _lastButton;
    private EditButton? _mirrorButton;
    private EditButton? _propertiesButton;
    private EditButton? _rotateButton;
    private EditButton? _selectAreaButton;
    private double ButtonHeight { get; set; }
    private StackLayout ButtonLayout { get; set; } = new();

    private double ButtonWidth { get; set; }

    public EditWindow(WindowContainer? layout) : base(layout, "Tools", "", false, GetView, titleHeight: 28)
    {
        _layout = layout;
        Initialize();
    }

    private static void CallHandler(string command)
    {
        API.Call(() =>
        {
            API.Instance.Call(command);
            return Task.CompletedTask;
        }).Wait();
    }

    private static void Delete()
    {
        CallHandler("delete");
    }

    private static View GetView(Window arg)
    {
        if (arg is EditWindow edit)
        {
            return edit.ButtonLayout;
        }

        return new Label();
    }

    private static void Mirror()
    {
        CallHandler("mirror");
    }

    private static void Rotate()
    {
        CallHandler("rotate");
    }

    private static void SelectArea()
    {
        CallHandler("selectarea");
    }

    private static void ShowProperties()
    {
        CallHandler("showproperties");
    }

    private void AddButtons()
    {
        if (ButtonHeight == 0) ButtonHeight = 60;
        if (ButtonWidth == 0) ButtonWidth = 88;

        _selectAreaButton = new EditButton("Select", SelectArea, OnSelectButtonChange, ButtonWidth, ButtonHeight, true);
        _propertiesButton =
            new EditButton("Properties", ShowProperties, OnSelectButtonChange, ButtonWidth, ButtonHeight);
        _rotateButton = new EditButton("Rotate", Rotate, OnSelectButtonChange, ButtonWidth, ButtonHeight);
        _mirrorButton = new EditButton("Mirror", Mirror, OnSelectButtonChange, ButtonWidth, ButtonHeight);
        _deleteButton = new EditButton("Delete", Delete, OnSelectButtonChange, ButtonWidth, ButtonHeight);
        ButtonLayout.Add(_selectAreaButton);
        ButtonLayout.Add(_propertiesButton);
        ButtonLayout.Add(_rotateButton);
        ButtonLayout.Add(_mirrorButton);
        ButtonLayout.Add(_deleteButton);
    }

    private void Initialize()
    {
        WidthRequest = 64;
        ButtonLayout = ButtonLayout
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill)
            .Margin(0)
            .Padding(0);
        Start();

        _layout?.SetWindowPosition(this, 4, 50);
        HideWindowButtons();
        HideResizer();
        AddButtons();
        _layout?.SetWindowSize(this, 100, 70 * ButtonLayout.Children.Count);
    }

    private void OnSelectButtonChange(EditButton editButton)
    {
        _lastButton?.Deselect();
        _lastButton = editButton;
    }
}
