using ACDCs.ApplicationLogic.Views.Menu;
using Microsoft.Maui.Layouts;

namespace ACDCs.ApplicationLogic.Components.Window;

#pragma warning disable IDE0065

using Sharp.UI;

#pragma warning restore IDE0065

public class Window : Grid
{
    private readonly WindowContainer _container;
    private readonly string _title;
    private readonly string? _menuFile;
    private AbsoluteLayout _childLayout;
    private Image _backgroundImage;
    private WindowTitle _windowTitle;
    private WindowButtons _windowButtons;
    private MenuView _menuView;

    public Window(WindowContainer container, string title, string? menuFile = null, bool isWindowParent = false)
    {
        _container = container;
        _title = title;
        _menuFile = menuFile;

        AddGridDefinitions();
        AddBackgroundImage();
        AddChildLayout(isWindowParent);
        AddWindowTitle();
        AddWindowButtons();

        if (_menuFile != null)
        {
            LoadMenu();
        }

        container.AddWindow(this);

        SetSize(500, 400);

        AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.None);
        OnClose = DefaultClose;
        Loaded += Window_Loaded;
    }

    private void LoadMenu()
    {
        _menuView = new MenuView(_menuFile)
        {
            HeightRequest = 34,
            ParentWindow = this
        };
        _childLayout.Add(_menuView);
    }

    private void SetSize(int width, int height)
    {
        _container.SetWindowSize(this, width, height);
    }

    private void AddWindowButtons()
    {
        _windowButtons = new WindowButtons(this);
        this.SetRowAndColumn(_windowButtons, 0, 1);
        Add(_windowButtons);
    }

    private void AddGridDefinitions()
    {
        this.ColumnDefinitions(new ColumnDefinitionCollection { new ColumnDefinition(), new ColumnDefinition(102) });

        this.RowDefinitions(new RowDefinitionCollection
        {
            new RowDefinition(38), new RowDefinition(), new RowDefinition(34)
        });
    }

    private void AddChildLayout(bool isWindowParent)
    {
        _childLayout = isWindowParent ? new WindowContainer() : new AbsoluteLayout();
        this.SetRowAndColumn(_childLayout, 1, 0, 2);
        Add(_childLayout);
    }

    private void AddWindowTitle()
    {
        _windowTitle = new WindowTitle(_title, this);
        this.SetRowAndColumn(Title, 0, 0, 2);
        Add(_windowTitle);
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

    public WindowTitle Title => _windowTitle;
    public int LastWidth { get; set; }
    public int LastHeight { get; set; }
    public double LastY { get; set; }
    public double LastX { get; set; }

    private void Window_Loaded(object? sender, EventArgs e)
    {
        GetBackgroundImage();
    }

    public void GetBackgroundImage()
    {
        Rect currentBounds = AbsoluteLayout.GetLayoutBounds(this);
        _backgroundImage.Source =
            API.Instance.WindowImageSource(Convert.ToSingle(currentBounds.Width),
                Convert.ToSingle(currentBounds.Height));
    }

    public void Minimize()
    {
        WindowState = WindowState.Minimized;
        _container.MinimizeWindow(this);
    }

    public WindowState WindowState { get; set; }

    public void Maximize()
    {
        WindowState = WindowState.Maximized;
        _container.MaximizeWindow(this);
        _windowButtons.ShowRestore();
    }

    public void Restore()
    {
        WindowState = WindowState.Standard;
        _container.RestoreWindow(this);
        _windowButtons.ShowMaximize();
    }

    public void Close()
    {
        if (OnClose.Invoke())
        {
            _container.CloseWindow(this);
        }
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public Func<bool> OnClose { get; set; }

    public AbsoluteLayout ChildLayout
    {
        get => _childLayout;
        set => _childLayout = value;
    }

    private bool DefaultClose()
    {
        return true;
    }
}
