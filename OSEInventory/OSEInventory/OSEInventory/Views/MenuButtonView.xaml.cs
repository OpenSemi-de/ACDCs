using AbsoluteLayout = Microsoft.Maui.Controls.AbsoluteLayout;
using Button = Microsoft.Maui.Controls.Button;

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
