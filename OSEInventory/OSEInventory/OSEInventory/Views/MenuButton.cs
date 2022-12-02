// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace OSEInventory.Views
{
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
}
