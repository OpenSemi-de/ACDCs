using ACDCs.Views.Window;

namespace ACDCs.Components.Menu;

using Sharp.UI;
using IMenuComponent = Interfaces.IMenuComponent;

public class MenuFrame : StackLayout
{
    public static List<MenuFrame> MenuFrameList = new();
    private readonly bool _eventSet;
    public View? MainContainer { get; set; }
    public AbsoluteLayout? PopupTarget { get; set; }
    public WindowView? WindowFrame { get; set; }

    public MenuFrame()
    {
        HorizontalOptions = LayoutOptions.Fill;
        VerticalOptions = LayoutOptions.Fill;
        Orientation = StackOrientation.Horizontal;
        if (!_eventSet)
        {
            API.Reset += App_Reset;
            _eventSet = true;
        }
    }

    public static void HideAllMenus()
    {
        MenuFrameList.ForEach(menuFrame => { menuFrame.IsVisible = false; });
    }

    public void LoadMenu(List<MenuItemDefinition> items, bool isRoot = false)
    {
        API.Call(() =>
        {
            if (!isRoot)
            {
                MenuFrameList.Add(this);
            }

            List<IMenuComponent> menuParts = new();
            foreach (MenuItemDefinition menuItem in items)
            {
                if (menuItem.Text != "" && menuItem.IsChecked == "")
                {
                    MenuButton menuButton = new(menuItem.Text, menuItem.MenuCommand, menuItem.ClickAction);
                    menuButton.ItemWidth = menuButton.Width;
                    menuParts.Add(menuButton);
                    if (menuItem.MenuItems != null && menuItem.MenuItems.Count > 0)
                    {
                        menuButton.MenuFrame = new()
                        {
                            PopupTarget = PopupTarget,
                            Orientation = isRoot
                                ? Orientation == StackOrientation.Horizontal
                                    ? StackOrientation.Vertical
                                    : StackOrientation.Horizontal
                                : Orientation,
                            MainContainer = MainContainer,
                            IsVisible = false
                        };

                        menuButton.MenuFrame.LoadMenu(menuItem.MenuItems);

                        PopupTarget?.Add(menuButton.MenuFrame);
                    }
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

    public void SetPosition(View menuButtonView)
    {
        if (menuButtonView is MenuButton menuButton)
        {
            if (menuButton.MenuFrame != null)
            {
                double childrenWidth = menuButton.MenuFrame.Children.Max(child => ((IMenuComponent)child).ItemWidth);
                if (childrenWidth < 100)
                    childrenWidth = 100;
                double childrenHeight = menuButton.MenuFrame.Children.Sum(child => ((IMenuComponent)child).ItemHeight);
                double mainX = Microsoft.Maui.Controls.AbsoluteLayout.GetLayoutBounds(MainContainer).X;
                double mainY = Microsoft.Maui.Controls.AbsoluteLayout.GetLayoutBounds(MainContainer).Y + Microsoft.Maui.Controls.AbsoluteLayout.GetLayoutBounds(MainContainer).Height;
                if (WindowFrame != null)
                {
                    mainX += AbsoluteLayout.GetLayoutBounds(WindowFrame).X;
                    mainY += AbsoluteLayout.GetLayoutBounds(WindowFrame).Y + menuButton.Height;
                    menuButton.MenuFrame.ZIndex(999);
                }

                Microsoft.Maui.Controls.AbsoluteLayout.SetLayoutBounds(menuButton.MenuFrame,
                    new(menuButton.X + mainX, mainY, childrenWidth + 2, childrenHeight));
            }
        }
    }

    private void App_Reset(object sender, ResetEventArgs args)
    {
        HideAllMenus();
    }
}
