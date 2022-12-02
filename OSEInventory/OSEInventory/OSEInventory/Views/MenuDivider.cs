// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace OSEInventory.Views
{
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
}
