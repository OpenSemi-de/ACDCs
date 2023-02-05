using ACDCs.ApplicationLogic.Interfaces;

namespace ACDCs.ApplicationLogic.Components.Menu;

#pragma warning disable IDE0065

using Sharp.UI;

#pragma warning restore IDE0065

public class MenuFrame : StackLayout
{
    private static readonly List<MenuFrame> s_menuFrameList = new();
    private readonly bool _eventSet;
    private List<MenuHandler> _handlers = new();
    public Window.Window ParentWindow { get; set; }

    public MenuFrame()
    {
        this.BackgroundColor(Colors.Transparent);

        HorizontalOptions = LayoutOptions.Fill;
        VerticalOptions = LayoutOptions.Fill;
        Orientation = StackOrientation.Horizontal;
        if (_eventSet)
        {
            return;
        }

        API.Reset += App_Reset;
        _eventSet = true;
    }

    public static void HideAllMenus()
    {
        s_menuFrameList.ForEach(menuFrame => { menuFrame.IsVisible = false; });
    }

    public void LoadMenu(List<MenuItemDefinition> items, bool isRoot = false, Dictionary<string, object> menuParameters = null)
    {
        API.Call(() =>
        {
            if (!isRoot)
            {
                s_menuFrameList.Add(this);
            }

            List<IMenuComponent> menuParts = new();
            foreach (MenuItemDefinition menuItem in items)
            {
                if (menuItem.Handler != "")
                {
                    LoadHandler(menuItem.Handler, menuParameters);
                }

                if (menuItem.Text != "" && menuItem.IsChecked == "")
                {
                    MenuButton menuButton = new(menuItem.Text, menuItem.MenuCommand, menuItem.ClickAction);

                    if (Orientation == StackOrientation.Horizontal)
                    {
                        menuButton.WidthRequest(80);
                        menuButton.ItemWidth = 84;
                    }

                    menuParts.Add(menuButton);
                    if (menuItem.MenuItems is not { Count: > 0 })
                    {
                        continue;
                    }

                    menuButton.MenuFrame = new MenuFrame
                    {
                        ParentWindow = ParentWindow,
                        Orientation = isRoot
                            ? Orientation == StackOrientation.Horizontal
                                ? StackOrientation.Vertical
                                : StackOrientation.Horizontal
                            : Orientation,
                        IsVisible = false
                    };

                    menuButton.MenuFrame.LoadMenu(menuItem.MenuItems, false, menuParameters);

                    ParentWindow.ChildLayout?.Add(menuButton.MenuFrame);
                }
                else if (menuItem.IsChecked != "")
                {
                    MenuToggle toggle = new(menuItem.Text, menuItem.MenuCommand);
                    menuParts.Add(toggle);
                }
                else
                {
                    menuParts.Add(new MenuDivider());
                }
            }

            menuParts.ForEach(part => Add(part as Microsoft.Maui.IView));
            return Task.CompletedTask;
        }).Wait();
    }

    private void LoadHandler(string menuItemHandler, Dictionary<string, object> menuParameters)
    {
        Type? handlerType = this.GetType().Assembly.GetTypes().FirstOrDefault(t => t.Name.Contains($"{menuItemHandler}Handlers"));
        if (handlerType == null)
            return;

        bool exists = _handlers.Any(h => h.GetType().Name.Contains(menuItemHandler));

        if (exists) return;

        MenuHandler? instance = Activator.CreateInstance(handlerType) as MenuHandler;
        if (instance == null)
            return;

        foreach (KeyValuePair<string, object> param in menuParameters)
        {
            instance.SetParameter(param.Key, param.Value);
        }

        _handlers.Add(instance);
    }

    public void SetPosition(View menuButtonView)
    {
        if (menuButtonView is not MenuButton menuButton)
        {
            return;
        }

        if (menuButton.MenuFrame == null)
        {
            return;
        }

        double mainX = 0;
        double mainY = menuButton.Height;
        mainX += AbsoluteLayout.GetLayoutBounds(ParentWindow.ChildLayout).X;
        mainY += AbsoluteLayout.GetLayoutBounds(ParentWindow.ChildLayout).Y + menuButton.Height;
        menuButton.MenuFrame.ZIndex(999);

        Microsoft.Maui.Controls.AbsoluteLayout.SetLayoutBounds(menuButton.MenuFrame,
                    new Rect(menuButton.X + mainX, mainY, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
    }

    private static void App_Reset(object sender, ResetEventArgs args)
    {
        HideAllMenus();
    }
}
