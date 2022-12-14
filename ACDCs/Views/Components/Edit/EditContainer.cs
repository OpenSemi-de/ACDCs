using ACDCs.Views.Components.CircuitView;
using Microsoft.Maui.Graphics;
using Sharp.UI;
using AbsoluteLayout = Sharp.UI.AbsoluteLayout;
using StackLayout = Sharp.UI.StackLayout;
using Microsoft.Maui.Layouts;
using ContentView = Sharp.UI.ContentView;

namespace ACDCs.Views.Components.Edit;

[BindableProperties]
public interface IEditContainerProperties
{
    CircuitViewContainer CircuitView { get; set; }
}


[SharpObject]
public partial class EditContainer : StackLayout, IEditContainerProperties
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

    private void SelectArea()
    { }

    private void Delete()
    { }

    private void Mirror()
    { }

    private void Rotate()
    { }

    private readonly EditButton _deleteButton;
    private readonly EditButton _mirrorButton;
    private readonly EditButton _rotateButton;
    private EditButton? _lastButton;
}

