using Microsoft.Maui;
using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACDCs.Views.Components.Menu;

public class MenuFrame : StackLayout
{
    public static List<MenuFrame> MenuFrameList = new();
    private bool _eventSet;

    public MenuFrame()
    {
        HorizontalOptions = LayoutOptions.Fill;
        VerticalOptions = LayoutOptions.Fill;
        Orientation = StackOrientation.Horizontal;
        if (!_eventSet)
        {
            App.Reset += App_Reset;
            _eventSet = true;
        }
    }

    public View MainContainer { get; set; }

    public AbsoluteLayout? PopupTarget { get; set; }

    public static void HideAllMenus()
    {
        MenuFrameList.ForEach(menuFrame => { menuFrame.IsVisible = false; });
    }

    public async void LoadMenu(List<MenuItemDefinition> items, bool isRoot = false)
    {
        App.Call(() =>
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
                    else
                    {
                        menuParts.Add(new MenuDivider());
                    }
                }

                menuParts.ForEach(part => Add((IView)part));
            }
            return Task.CompletedTask;
        }).Wait();
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
                AbsoluteLayout.SetLayoutBounds(menuButton.MenuFrame,
                    new(menuButton.X + mainX, 50, 140, childrenHeight));
            }
        }
    }

    private void App_Reset(object sender, ResetEventArgs args)
    {
        MenuFrame.HideAllMenus();
    }
}