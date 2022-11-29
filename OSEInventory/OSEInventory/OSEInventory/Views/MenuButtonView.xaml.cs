using Microsoft.Maui.Layouts;
using AbsoluteLayout = Microsoft.Maui.Controls.AbsoluteLayout;
using Button = Microsoft.Maui.Controls.Button;
using ScrollView = Microsoft.Maui.Controls.ScrollView;

namespace OSEInventory.Views;

public partial class MenuButtonView : ContentView
{
    private readonly List<MenuPopup> _menus = new();

    public MenuButtonView()
    {
        InitializeComponent();
        Loaded += OnLoaded;
        App.DoReset += App_DoReset;
        App.MenuButtonView = this;
    }

    public static Dictionary<string, Action> Handlers { get; set; } = new();

    public void HideAllMenus()
    {
        _menus.ForEach(m => m.IsVisible = false);
    }

    public static Action? GetHandler(string? menuCommand)
    {
        if (menuCommand != null && Handlers.ContainsKey(menuCommand))
        {
            return Handlers[menuCommand];
        }
        return null;
    }

    private void App_DoReset(DoResetArgs? args)
    {
        HideAllMenus();
    }

    private void AttachClick(List<MenuButton> fileMenu)
    {
        fileMenu.ForEach(menu => menu.Clicked = OnClick);
    }

    private MenuPopup CreateMenu(AbsoluteLayout absoluteLayout, Button button)
    {
        MenuPopup menuPopup = new(button.Text, button, this);
        absoluteLayout.Add(menuPopup);
        button.Clicked += (sender, args) =>
        {
            HideAllMenus();
            menuPopup.IsVisible = true;
        };
        _menus.Add(menuPopup);
        ZIndex++;
        return menuPopup;
    }

    private void OnClick(string? menuCommand)
    {
        MenuButtonView.GetHandler(menuCommand)?.Invoke();
    }

    private void OnLoaded(object? sender, EventArgs e)
    {
        if (Parent is AbsoluteLayout absoluteLayout)
        {
            List<MenuButton> fileMenu = new()
            {
                new () { Text = "Open...", MenuCommand = "open"},
                new () { Text = "Save", MenuCommand = " save"},
                new () { Text = "Save as...", MenuCommand = "saveas"},
            };
            AttachClick(fileMenu);
            CreateMenu(absoluteLayout, ButtonFileMenu).SetItems(fileMenu);
            CreateMenu(absoluteLayout, ButtonEditMenu);
        }
    }

    public void SetHandler(string command, Action commandAction)
    {
        Handlers.Add(command, commandAction);
    }
}

public class MenuPopup : Frame
{
    private readonly StackLayout _baseLayout;
    private readonly string _name;
    private readonly ScrollView _scroll;
    private List<MenuButton>? _buttons;
    private Rect _hookPosition;

    public MenuPopup(string name, Button openButton, MenuButtonView controlButtonView)
    {
        _hookPosition = new Rect()
        {
            Top = 1,
            Left = openButton.X,
            Width = 100,
            Height = 100
        };

        AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.YProportional);
        AbsoluteLayout.SetLayoutBounds(this, _hookPosition);

        BorderColor = Application.AccentColor;
        CornerRadius = 2;
        Padding = 1;
        IsVisible = false;

        _name = name;
        _scroll = new ScrollView();
        _baseLayout = new StackLayout();
        _scroll.Content = _baseLayout;

        Content = _scroll;
    }

    public Action<string>? Clicked { get; set; }

    public void SetItems(IEnumerable<MenuButton>? buttons)
    {
        if (buttons != null)
        {
            IEnumerable<MenuButton> menuButtons = buttons.ToList();
            _buttons = menuButtons.ToList();
            foreach (MenuButton menuButton in menuButtons)
            {
                _baseLayout.Add(menuButton);
            }

            var backButton = new MenuButton
            {
                Text = " < Back",
                Clicked = (menuCommand) =>
                {
                    IsVisible = false;
                    if (menuCommand != null)
                    {
                        this.Clicked?.Invoke(menuCommand);
                    }
                }
            };
            _baseLayout.Add(backButton);
            HeightRequest = 40 * menuButtons.Count() + 120;
            _hookPosition.Height = HeightRequest;
            AbsoluteLayout.SetLayoutBounds(this, _hookPosition);
        }
    }
}

public class MenuButton : Label, IBorder
{
    private readonly TapGestureRecognizer _tapRecognizer;

    public MenuButton()
    {
        Padding = 2;
        HeightRequest = 40;
        HorizontalTextAlignment = TextAlignment.Start;
        VerticalTextAlignment = TextAlignment.Center;
        FontAttributes = FontAttributes.Bold;

        _tapRecognizer = new TapGestureRecognizer();
        _tapRecognizer.Tapped += (sender, args) =>
        {
            Clicked?.Invoke(this.MenuCommand);
        };
        GestureRecognizers.Add(_tapRecognizer);
    }

    public IBorderStroke Border { get; } = new Border();
    public Action<string?>? Clicked { get; set; }
    public string MenuCommand { get; set; }
    public MenuPopup? Popup { get; set; }
}
