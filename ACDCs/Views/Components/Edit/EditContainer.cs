using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;

namespace ACDCs.Views.Components.Edit;

public class EditContainer : StackLayout
{
    public EditContainer()
    {
        AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.None);
        AbsoluteLayout.SetLayoutBounds(this, new Rect(0,300,AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
        _selectAreaButton = new(SelectArea, OnSelectButtonChange);
        _rotateButton = new(Rotate, OnSelectButtonChange);
        _mirrorButton = new(Mirror, OnSelectButtonChange);
        _deleteButton = new(Delete, OnSelectButtonChange);
        Add(_selectAreaButton);
        Add(_rotateButton);
        Add(_mirrorButton);
        Add(_deleteButton);
    }

    private readonly EditButton _selectAreaButton;

    private void OnSelectButtonChange(EditButton editButton)
    {
        _lastButton?.Deselect();
        _lastButton = editButton;
    }

    public void SelectArea()
    { }

    public void Delete()
    { }

    public void Mirror()
    { }

    public void Rotate()
    { }

    private readonly EditButton _deleteButton;
    private readonly EditButton _mirrorButton;
    private readonly EditButton _rotateButton;
    private EditButton? _lastButton;
}
