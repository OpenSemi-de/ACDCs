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
        ButtonEditMenu.BackgroundColor = Colors.WhiteSmoke;
        ButtonFileMenu.BackgroundColor = Colors.WhiteSmoke;
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

    private void AttachClick(List<IMenuItem> menu)
    {
        menu.ForEach(menuItem =>
        {
            if (menuItem is MenuButton button)
            {
                button.Clicked = OnClick;

                if (button.MenuCommand == "")
                {
                    var menuCommand = button.Text;
                    menuCommand = menuCommand.Replace(".", "").Replace(" ", "").ToLower();
                    button.MenuCommand = menuCommand;
                }
            }
        });
    }

    private MenuPopup CreateMenu(AbsoluteLayout absoluteLayout, Button button)
    {
        MenuPopup menuPopup = new(button.Text, button, this);
        absoluteLayout.Add(menuPopup);
        button.Clicked += (sender, args) =>
        {
            HideAllMenus();
            menuPopup.IsVisible = true;
            BackgroundColor = Colors.LightSkyBlue;
        };
        _menus.Add(menuPopup);
        ZIndex++;
        return menuPopup;
    }

    private void OnClick(string? menuCommand)
    {
        MenuButtonView.GetHandler(menuCommand)?.Invoke();
        HideAllMenus();
    }

    private void OnLoaded(object? sender, EventArgs e)
    {
        if (Parent is AbsoluteLayout absoluteLayout)
        {
            List<IMenuItem> fileMenu = new()
            {
                new MenuButton() { Text = "Open..."},
                new MenuButton() { Text = "Save" },
                new MenuButton() { Text = "Save as..." },
            };
            AttachClick(fileMenu);

            List<IMenuItem> editMenu = new()
            {
                new MenuButton(){Text = "Duplicate"},
                new MenuButton() { Text = "Delete" },
                new MenuDivider(),
                new MenuButton() { Text = "Copy" },
                new MenuButton() { Text = "Cut" },
                new MenuButton() { Text = "Paste" }
            };
            AttachClick(editMenu);

            CreateMenu(absoluteLayout, ButtonFileMenu).SetItems(fileMenu);
            CreateMenu(absoluteLayout, ButtonEditMenu).SetItems(editMenu);
        }
    }

    public void SetHandler(string command, Action commandAction)
    {
        if (!Handlers.ContainsKey(command))
            Handlers.Add(command, commandAction);
    }
}

public class MenuPopup : Frame
{
    private readonly StackLayout _baseLayout;
    private readonly string _name;
    private readonly ScrollView _scroll;
    private List<IMenuItem>? _items;
    private Rect _hookPosition;

    public MenuPopup(string name, Button openButton, MenuButtonView controlButtonView)
    {
        _hookPosition = new Rect()
        {
            Top = 1,
            Left = openButton.X,
            Width = openButton.Width,
            Height = 100
        };

        AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.YProportional);
        AbsoluteLayout.SetLayoutBounds(this, _hookPosition);
        Padding = 0;
        IsVisible = false;

        _name = name;
        _scroll = new ScrollView();
        _baseLayout = new StackLayout();
        _scroll.Content = _baseLayout;
        
        Content = _scroll;
    }

    public Action<string>? Clicked { get; set; }

    public void SetItems(IEnumerable<IMenuItem>? items)
    {
        if (items != null)
        {
            IEnumerable<IMenuItem> menuItems = items.ToList();
            _items = menuItems.ToList();

            double height = 0;
            foreach (IMenuItem item in _items)
            {
                height += ((IView)item).Height;
                _baseLayout.Add((IView)item);
            }

            HeightRequest = height + 80;
            _hookPosition.Height = HeightRequest;
            AbsoluteLayout.SetLayoutBounds(this, _hookPosition);
        }
    }
}

public interface IMenuItem
{
}

public class MenuDivider : Label, IBorder, IMenuItem
{
    public MenuDivider()
    {
        HeightRequest = 1;
        BackgroundColor = Colors.Black;
        Padding = 0;
        Margin = 1;
        
    }

    public IBorderStroke Border { get; } = new Border();
}

public class MenuButton : Label, IBorder, IMenuItem
{
    private readonly TapGestureRecognizer _tapRecognizer;

    public MenuButton()
    {
        Padding = new Thickness(6, 2, 2, 2);
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
    public string MenuCommand { get; set; } = "";
    public MenuPopup? Popup { get; set; }
}
