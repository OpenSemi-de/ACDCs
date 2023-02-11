namespace ACDCs.ApplicationLogic.Components.Menu;

using Delegates;
using Interfaces;
using Sharp.UI;
using Window;
using AbsoluteLayout = AbsoluteLayout;
using IView = IView;

public class MenuFrame : StackLayout
{
    private static readonly List<MenuFrame> s_menuFrameList = new();
    private readonly bool _eventSet;
    private readonly List<MenuHandler> _handlers = new();
    public Window? ParentWindow { get; set; }

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

    public void LoadMenu(List<MenuItemDefinition> items, bool isRoot = false, Dictionary<string, object>? menuParameters = null)
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
                        IsVisible = false,
                        ZIndex = 10
                    };

                    menuButton.MenuFrame.LoadMenu(menuItem.MenuItems, false, menuParameters);

                    ParentWindow?.ChildLayout.Add(menuButton.MenuFrame);
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

            menuParts.ForEach(part => Add(part as IView));
            return Task.CompletedTask;
        }).Wait();
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
        mainX += Sharp.UI.AbsoluteLayout.GetLayoutBounds(ParentWindow?.ChildLayout).X;
        mainY += Sharp.UI.AbsoluteLayout.GetLayoutBounds(ParentWindow?.ChildLayout).Y + menuButton.Height;
        menuButton.MenuFrame.ZIndex(999);

        AbsoluteLayout.SetLayoutBounds(menuButton.MenuFrame,
                    new Rect(menuButton.X + mainX, mainY, Sharp.UI.AbsoluteLayout.AutoSize, Sharp.UI.AbsoluteLayout.AutoSize));
    }

    private static void App_Reset(object sender, ResetEventArgs args)
    {
        HideAllMenus();
    }

    private static void HideAllMenus()
    {
        s_menuFrameList.ForEach(menuFrame => { menuFrame.IsVisible = false; });
    }

    private void LoadHandler(string menuItemHandler, Dictionary<string, object> menuParameters)
    {
        Type? handlerType = GetType().Assembly.GetTypes().FirstOrDefault(t => t.Name.Contains($"{menuItemHandler}Handlers"));
        if (handlerType == null)
            return;

        bool exists = _handlers.Any(h => h.GetType().Name.Contains(menuItemHandler));

        if (exists) return;

        if (Activator.CreateInstance(handlerType) is not MenuHandler instance)
            return;

        foreach (KeyValuePair<string, object> param in menuParameters)
        {
            instance.SetParameter(param.Key, param.Value);
        }

        _handlers.Add(instance);
    }
}
