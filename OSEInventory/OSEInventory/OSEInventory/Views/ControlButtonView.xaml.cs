namespace OSEInventory.Views;

public partial class ControlButtonView : ContentView
{
    private ImageButton _selectedToolButton;
    private readonly Color _selectedToolButtonColor;
    public SelectedControlType SelectedControlType { get; private set; }
    private readonly Dictionary<ImageButton, SelectedControlType> buttonControlTypes = new();
    public ControlButtonView()
	{
        InitializeComponent();
        buttonControlTypes = new()  {
            {bnSelectTool, SelectedControlType.ItemSelection},
            {bnMoveTool, SelectedControlType.ItemMovement},
            {bnRotateTool, SelectedControlType.ItemRotate}
        };


        _selectedToolButtonColor = bnSelectTool.BackgroundColor;
        _selectedToolButton = bnSelectTool;
        SelectedControlType = SelectedControlType.ItemSelection;
        SelectToolButton(bnSelectTool);
    }
    private void BnMoveTool_OnClicked(object? sender, EventArgs e)
    {
        SelectToolButton(sender);
    }

    private void BnRotateTool_OnClicked(object? sender, EventArgs e)
    {
        SelectToolButton(sender);
    }

    private void BnSelectTool_OnClicked(object? sender, EventArgs e)
    {
        SelectToolButton(sender);
    }

    private void SelectToolButton(object? button)
    {
        if (button != null)
        {
            _selectedToolButton.BackgroundColor = _selectedToolButtonColor;

            _selectedToolButton = (ImageButton)button;
            _selectedToolButton.BackgroundColor = Colors.LightSkyBlue;
            SelectedControlType = buttonControlTypes[(ImageButton)button];
        }
    }
}

public enum SelectedControlType
{
    ItemSelection,
    ItemMovement,
    ItemRotate
}
