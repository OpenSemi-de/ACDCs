namespace ACDCs.ApplicationLogic.Components.Window;

using Menu;
using Microsoft.Maui.Layouts;
using Sharp.UI;

public class Window : Grid
{
    private readonly Func<Window, View> _childViewFunction;
    private readonly WindowContainer? _container;
    private readonly bool _isWindowParent;
    private readonly string? _menuFile;
    private readonly string _title;
    private Image _backgroundImage;
    private AbsoluteLayout _childLayout;
    private View? _childView;
    private MenuView _menuView;
    private WindowButtons _windowButtons;
    private WindowTitle _windowTitle;

    public AbsoluteLayout ChildLayout
    {
        get => _childLayout;
        set => _childLayout = value;
    }

    public View? ChildView
    {
        get => _childView;
    }

    public int LastHeight { get; set; }

    public int LastWidth { get; set; }

    public WindowState? LastWindowState { get; private set; } = WindowState.Standard;
    public double LastX { get; set; }

    public double LastY { get; set; }

    public WindowContainer? MainContainer
    {
        get
        {
            if (_childLayout is WindowContainer childlayout) return childlayout;
            return _container;
        }
    }

    public Dictionary<string, object> MenuParameters { get; } = new();

    public MenuView MenuView
    {
        get => _menuView;
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public Func<bool> OnClose { get; set; }

    public Action<Window>? Started { get; set; }

    public WindowTitle Title => _windowTitle;

    public WindowState WindowState { get; set; } = WindowState.Standard;

    public string WindowTitle
    {
        get => _title;
    }

    public Window(WindowContainer? container, string title, string? menuFile = null, bool isWindowParent = false,
                                    Func<Window, View> childViewFunction = null)
    {
        _container = container;
        _title = title;
        _menuFile = menuFile;
        _isWindowParent = isWindowParent;
        _childViewFunction = childViewFunction;
    }

    public void Close()
    {
        if (OnClose.Invoke())
        {
            _container?.CloseWindow(this);
        }
    }

    public void GetBackgroundImage()
    {
        Rect currentBounds = AbsoluteLayout.GetLayoutBounds(this);
        _backgroundImage.Source =
            API.Instance.WindowImageSource(Convert.ToSingle(currentBounds.Width),
                Convert.ToSingle(currentBounds.Height));
    }

    public void Maximize()
    {
        WindowState = WindowState.Maximized;
        LastWindowState = WindowState;
        _container?.MaximizeWindow(this);
        _windowButtons.ShowRestore();
    }

    public void Minimize()
    {
        WindowState = WindowState.Minimized;
        _container?.MinimizeWindow(this);
    }

    public void Restore()
    {
        _container?.RestoreWindow(this);
        _windowButtons.ShowMaximize();
    }

    public void SetActive()
    {
    }

    public void SetInactive()
    {
    }

    public void Start()
    {
        AddGridDefinitions();
        AddBackgroundImage();
        AddChildLayout(_isWindowParent);
        AddWindowTitle();
        AddWindowButtons();

        if (_menuFile != null)
        {
            LoadMenu();
        }

        _container?.AddWindow(this);

        SetSize(500, 400);
        _childView = _childViewFunction?.Invoke(this);
        if (_childView != null)
        {
            AbsoluteLayout.SetLayoutFlags(_childView, AbsoluteLayoutFlags.SizeProportional);
            AbsoluteLayout.SetLayoutBounds(_childView, new Rect(0, 0, 1, 1));
            _childLayout?.Add(_childView);
        }
        AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.None);
        OnClose = DefaultClose;
        Loaded += Window_Loaded;
    }

    protected void HideResizer()
    {
    }

    protected void HideWindowButtons()
    {
        _windowButtons.IsVisible = false;
    }

    private void AddBackgroundImage()
    {
        _backgroundImage = new Image()
            .Margin(0)
            .Aspect(Aspect.Fill)
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill);
        this.SetRowAndColumn(_backgroundImage, 0, 0, 3, 3);
        Add(_backgroundImage);
    }

    private void AddChildLayout(bool isWindowParent)
    {
        _childLayout = isWindowParent ? new WindowContainer() : new AbsoluteLayout();
        this.SetRowAndColumn(_childLayout, 1, 0, 2, 2);
        Add(_childLayout);
    }

    private void AddGridDefinitions()
    {
        this.ColumnDefinitions(new ColumnDefinitionCollection { new ColumnDefinition(), new ColumnDefinition(102) });

        this.RowDefinitions(new RowDefinitionCollection
        {
            new RowDefinition(38), new RowDefinition(), new RowDefinition(34)
        });
    }

    private void AddWindowButtons()
    {
        _windowButtons = new WindowButtons(this);
        this.SetRowAndColumn(_windowButtons, 0, 1);
        Add(_windowButtons);
    }

    private void AddWindowTitle()
    {
        _windowTitle = new WindowTitle(_title, this);
        this.SetRowAndColumn(Title, 0, 0, 2);
        Add(_windowTitle);
    }

    private bool DefaultClose()
    {
        return true;
    }

    private void LoadMenu()
    {
        _menuView = new MenuView(_menuFile, MenuParameters)
        {
            PopupTarget = _childLayout,
            HeightRequest = 34,
            ParentWindow = this
        };
        _childLayout.Add(_menuView);
        _menuView.ZIndex = 999;
    }

    private void SetSize(int width, int height)
    {
        _container?.SetWindowSize(this, width, height);
    }

    private void Window_Loaded(object? sender, EventArgs e)
    {
        GetBackgroundImage();
        Started?.Invoke(this);
    }
}
