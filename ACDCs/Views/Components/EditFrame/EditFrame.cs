using System;
using ACDCs.CircuitRenderer.Drawables;
using ACDCs.CircuitRenderer.Instructions;
using ACDCs.CircuitRenderer.Interfaces;
using ACDCs.CircuitRenderer.Items;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace ACDCs.Views.Components.Feedback
{
    public class EditButton : ImageButton
    {
        public EditButton(Action action)
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

    public class EditFrame : AbsoluteLayout
    {
        public EditFrame()
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

        private readonly EditButton _deleteButton;
        private readonly EditButton _mirrorButton;
        private readonly EditButton _rotateButton;
    }
    
}

public class SelectAreaItem : WorksheetItem
{
    public SelectAreaItem()
    {
        DrawableComponent = new SelectAreaDrawable(this);
    }

    public static new bool IsInsertable => false;
}

public class SelectAreaDrawable : DrawableComponent
{
    public SelectAreaDrawable(IWorksheetItem parentItem) : base(typeof(SelectAreaDrawable), parentItem)
    {
        DrawInstructions.Add(
            new BoxInstruction(0.1f,0.1f, 0.5f,0.5f));
        SetSize(1, 1);
    }
}

