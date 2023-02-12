namespace ACDCs.ApplicationLogic.Components.Window;

using Menu;
using Microsoft.Maui.Layouts;
using Sharp.UI;

public class Window : ContentView
{
    private readonly Func<Window, View>? _childViewFunction;
    private readonly WindowContainer? _container;
    private readonly bool _isWindowParent;
    private readonly string? _menuFile;
    private readonly int _titleHeight;
    private Image? _backgroundImage;
    private View? _childView;
    private Grid? _mainGrid;
    private WindowButtons? _windowButtons;
    public AbsoluteLayout ChildLayout { get; private set; }
    public int LastHeight { get; set; }
    public int LastWidth { get; set; }
    public WindowState? LastWindowState { get; private set; } = WindowState.Standard;
    public double LastX { get; set; }
    public double LastY { get; set; }

    public WindowContainer? MainContainer
    {
        get
        {
            if (ChildLayout is WindowContainer childlayout) return childlayout;
            return _container;
        }
    }

    public Dictionary<string, object> MenuParameters { get; } = new();

    // ReSharper disable once MemberCanBePrivate.Global
    public Func<bool> OnClose { get; set; }

    public WindowResizer Resizer { get; private set; }

    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public Action<Window>? Started { get; set; }

    public WindowTitle Title { get; private set; }
    public WindowState WindowState { get; set; } = WindowState.Standard;
    public string WindowTitle { get; }
    protected View CurrentView { get; private set; }
    private MenuView MenuView { get; set; }

    protected Window(WindowContainer? container, string title, string? menuFile = null, bool isWindowParent = false,
                                    Func<Window, View>? childViewFunction = null, int titleHeight = 38)
    {
        _container = container;
        _titleHeight = titleHeight;
        WindowTitle = title;
        _menuFile = menuFile;
        _isWindowParent = isWindowParent;
        if (childViewFunction != null)
        {
            _childViewFunction = childViewFunction;
        }
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
        if (_backgroundImage != null)
        {
            _backgroundImage.Source =
                API.Instance.WindowImageSource(Convert.ToSingle(currentBounds.Width),
                    Convert.ToSingle(currentBounds.Height), _titleHeight);
        }
    }

    public void Maximize()
    {
        WindowState = WindowState.Maximized;
        LastWindowState = WindowState;
        _container?.MaximizeWindow(this);
        _windowButtons?.ShowRestore();
    }

    public void Minimize()
    {
        WindowState = WindowState.Minimized;
        _container?.MinimizeWindow(this);
    }

    public void Restore()
    {
        _container?.RestoreWindow(this);
        _windowButtons?.ShowMaximize();
    }

    public void SetActive()
    {
    }

    public void SetInactive()
    {
    }

    protected void HideResizer()
    {
        Resizer.IsVisible = false;
    }

    protected void HideWindowButtons()
    {
        if (_windowButtons != null)
        {
            _windowButtons.IsVisible = false;
        }
    }

    protected void HideWindowButtonsExceptClose()
    {
        _windowButtons?.ShowOnlyClose();
    }

    protected void Start()
    {
        AddGridDefinitions();
        AddBackgroundImage();
        AddChildLayout(_isWindowParent);
        AddWindowTitle();
        AddWindowButtons();
        AddWindowResizer();

        if (!string.IsNullOrEmpty(_menuFile))
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
            ChildLayout?.Add(_childView);
            CurrentView = _childView;
        }
        AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.None);
        OnClose = DefaultClose;
        Loaded += Window_Loaded;
        Content = _mainGrid;
    }

    private void AddBackgroundImage()
    {
        _backgroundImage = new Image()
            .Margin(0)
            .Aspect(Aspect.Fill)
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill);
        if (_mainGrid != null)
        {
            _mainGrid.SetRowAndColumn(_backgroundImage, 0, 0, 3, 3);
            _mainGrid.Add(_backgroundImage);
        }

        GetBackgroundImage();
    }

    private void AddChildLayout(bool isWindowParent)
    {
        ChildLayout = new WindowContainer();// : new AbsoluteLayout();
        if (_mainGrid != null)
        {
            _mainGrid.SetRowAndColumn(ChildLayout, 1, 0, 2, 2);
            _mainGrid.Add(ChildLayout);
        }
    }

    private void AddGridDefinitions()
    {
        _mainGrid = new Grid()
        .ColumnDefinitions(new ColumnDefinitionCollection
        {
            new ColumnDefinition(),
            new ColumnDefinition(102)
        })
        .RowDefinitions(new RowDefinitionCollection
        {
            new RowDefinition(_titleHeight), new RowDefinition(), new RowDefinition(34)
        });
    }

    private void AddWindowButtons()
    {
        _windowButtons = new WindowButtons(this);
        if (_mainGrid != null)
        {
            _mainGrid.SetRowAndColumn(_windowButtons, 0, 1);
            _mainGrid.Add(_windowButtons);
        }
    }

    private void AddWindowResizer()
    {
        Resizer = new WindowResizer()
            .Text("//")
            .ZIndex(999)
            .Row(2)
            .Column(1)
            .FontSize(20)
            .HorizontalOptions(LayoutOptions.End)
            .VerticalOptions(LayoutOptions.End);
        Resizer.ParentWindow = this;
        _mainGrid?.Add(Resizer);
    }

    private void AddWindowTitle()
    {
        Title = new WindowTitle(WindowTitle, this);
        if (_mainGrid != null)
        {
            _mainGrid.SetRowAndColumn(Title, 0, 0, 2);
            _mainGrid.Add(Title);
        }
    }

    private bool DefaultClose()
    {
        Task.Run(() =>
        {
            this.FadeTo(0).Wait();
        });

        return true;
    }

    private void LoadMenu()
    {
        MenuView = new MenuView(_menuFile, MenuParameters)
        {
            HeightRequest = 34,
            ParentWindow = this
        };
        ChildLayout.Add(MenuView);
        MenuView.ZIndex = 999;
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
