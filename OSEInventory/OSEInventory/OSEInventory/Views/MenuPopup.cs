// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Maui.Layouts;

namespace OSEInventory.Views
{
    public class MenuPopup : Frame
    {
        private readonly StackLayout _baseLayout;
        private readonly string _name;
        private readonly ScrollView _scroll;
        private Rect _hookPosition;
        private List<IMenuItem>? _items;

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
}
