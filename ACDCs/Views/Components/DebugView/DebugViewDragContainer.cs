using System;
using ACDCs.Views.Components.CircuitView;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;
using CommunityToolkit.Maui.Markup;

namespace ACDCs.Views.Components.DebugView
{
    public class DebugViewDragComtainer: DragContainer.DragContainer
    {
        private Point _cursorPosition;
        private readonly StackLayout _layout;
        private readonly Label _labelCursorPosition;
        private readonly Label _labelTapPosition;
        private Point _tapPosition;

        public DebugViewDragComtainer()
        {
            Title = "Debug";
            Orientation = StackOrientation.Vertical;
            ShowButtonHide();

            AbsoluteLayout.SetLayoutBounds(this, new(1, 400, 300, 400));
            AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.XProportional);

            _layout = new()
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                MinimumWidthRequest = 300
            };

            _layout.Add(new Label().Text("CursorPosition"));
            _labelCursorPosition = new Label() { HorizontalOptions = LayoutOptions.Fill, };
            _layout.Add(_labelCursorPosition);

            _layout.Add(new Label().Text("TapPosition"));
            _labelTapPosition = new Label() { HorizontalOptions = LayoutOptions.Fill, };
            _layout.Add(_labelTapPosition);


            Layout = _layout;

            Loaded += OnLoaded;
        }

        private void OnLoaded(object? sender, EventArgs e)
        {
            if (CircuitView != null)
            {
                CircuitView.CursorPositionChanged += OnCursorPositionChanged;
                CircuitView.TapPositionChanged += CircuitViewOnTapPositionChanged;
            }
        }

        private void CircuitViewOnTapPositionChanged(object sender, CursorPositionChangeEventArgs args)
        {
            _tapPosition = args.CursorPosition;
            _labelTapPosition.Text($"{_cursorPosition.X}/{_cursorPosition.Y}");
        }

        private void OnCursorPositionChanged(object sender, CursorPositionChangeEventArgs args)
        {
            _cursorPosition = args.CursorPosition;
            _labelCursorPosition.Text($"{_cursorPosition.X}/{_cursorPosition.Y}");
        }
    }
}
