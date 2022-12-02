using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace ACDCs.Views.Components.Menu;

public class MenuFrame : StackLayout
{
    public static List<MenuFrame> MenuFrameList = new();
    public MenuFrame()
    {
        HorizontalOptions = LayoutOptions.Fill;
        VerticalOptions = LayoutOptions.Fill;
        Orientation = StackOrientation.Horizontal;
    }

    public AbsoluteLayout PopupTarget { get; set; }
    public View MainContainer { get; set; }
    public async void LoadMenu(List<MenuItemDefinition> items, bool isRoot = false)
    {
        if (!isRoot)
        {
            MenuFrameList.Add(this);
        }

        List<IMenuItem> menuParts = new();
        if (items != null)
        {
            foreach (var menuItem in items)
            {
                if (menuItem.Text != "")
                {
                    var menuButton = new MenuButton(menuItem.Text, menuItem.MenuCommand);

                    menuParts.Add(menuButton);
                    if (menuItem.MenuItems != null && menuItem.MenuItems.Count > 0)
                    {
                        menuButton.MenuFrame = new MenuFrame
                        {
                            PopupTarget = PopupTarget,
                            Orientation = isRoot ? Orientation == StackOrientation.Horizontal
                                ? StackOrientation.Vertical
                                : StackOrientation.Horizontal : Orientation,
                            MainContainer = MainContainer,
                            IsVisible = false
                        };

                        menuButton.MenuFrame.LoadMenu(menuItem.MenuItems);

                        PopupTarget.Add(menuButton.MenuFrame);
                       // menuButton.MenuFrame.Loaded += (sender, args) => { SetPosition(menuButton); };
                      
                    }
                }
                else
                {
                    menuParts.Add(new MenuDivider());
                }
            }
            menuParts.ForEach(part => Add((IView)part));
        }
    }

    public void SetPosition(View menuButtonView)
    {
        var menuButton = menuButtonView as MenuButton;
        if (menuButton != null)
        {
            if (menuButton.MenuFrame != null)
            {
                var childrenHeight = menuButton.MenuFrame.Children.Sum(child => ((IMenuItem)child).ItemHeight);
                var mainX = AbsoluteLayout.GetLayoutBounds(MainContainer).X;
                AbsoluteLayout.SetLayoutBounds(menuButton.MenuFrame, new(menuButton.X + mainX, 50, 140, childrenHeight));
            }
        }
    }

    public static void HideAllMenus()
    {
        MenuFrameList.ForEach(menuFrame =>
        {
            menuFrame.IsVisible = false;
        });

    }
}