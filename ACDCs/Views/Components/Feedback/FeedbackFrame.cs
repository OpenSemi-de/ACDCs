using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace ACDCs.Views.Components.Feedback
{
    public class FeedbackButton : ImageButton
    {
        public FeedbackButton(Action action)
        {
            _action = action;
            Clicked += OnClicked;
            BackgroundColor = Colors.Transparent;
        }

        private readonly Action _action;

        private void OnClicked(object? sender, EventArgs e)
        {
            _action.Invoke();
        }
    }

    public class FeedbackFrame : AbsoluteLayout
    {
        public FeedbackFrame()
        {
            _rotateButton = new(Rotate);
            _mirrorButton = new(Mirror);
            _deleteButton = new(Delete);
        }

        public void Delete()
        { }

        public void Mirror()
        { }

        public void Rotate()
        { }

        private readonly FeedbackButton _deleteButton;
        private readonly FeedbackButton _mirrorButton;
        private readonly FeedbackButton _rotateButton;
    }
}
