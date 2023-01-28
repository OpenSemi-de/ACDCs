using ACDCs.Interfaces;
using ACDCs.Services;
using WindowView = ACDCs.Components.Window.WindowView;

namespace ACDCs.Components.Menu;

using Sharp.UI;

public class MenuFrame : StackLayout
{
    private static readonly List<MenuFrame> s_menuFrameList = new();
    private readonly bool _eventSet;
    public View? MainContainer { get; set; }
    public AbsoluteLayout? PopupTarget { get; set; }
    public WindowView? WindowFrame { get; set; }

    public MenuFrame()
    {
        BackgroundColor = ColorService.BackgroundHigh;
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

    public void LoadMenu(List<MenuItemDefinition> items, bool isRoot = false)
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
        if (menuButtonView is not MenuButton menuButton)
        {
            return;
        }

        if (menuButton.MenuFrame == null)
        {
            return;
        }

        double mainX = Microsoft.Maui.Controls.AbsoluteLayout.GetLayoutBounds(MainContainer).X;
        double mainY = Microsoft.Maui.Controls.AbsoluteLayout.GetLayoutBounds(MainContainer).Y + Microsoft.Maui.Controls.AbsoluteLayout.GetLayoutBounds(MainContainer).Height;
        if (WindowFrame != null)
        {
            mainX += AbsoluteLayout.GetLayoutBounds(WindowFrame).X;
            mainY += AbsoluteLayout.GetLayoutBounds(WindowFrame).Y + menuButton.Height;
            menuButton.MenuFrame.ZIndex(999);
        }

        Microsoft.Maui.Controls.AbsoluteLayout.SetLayoutBounds(menuButton.MenuFrame,
                    new Rect(menuButton.X + mainX, mainY, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
    }

    private static void App_Reset(object sender, ResetEventArgs args)
    {
        HideAllMenus();
    }
}
