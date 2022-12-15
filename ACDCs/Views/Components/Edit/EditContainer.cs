using ACDCs.Views.Components.CircuitView;
using Microsoft.Maui.Layouts;
using static Sharp.UI.AbsoluteLayout;

namespace ACDCs.Views.Components.Edit;

using Sharp.UI;

[BindableProperties]
public interface IEditContainerProperties
{
    CircuitViewContainer CircuitView { get; set; }
    double ButtonWidth { get; set; }
    double ButtonHeight { get; set; }
}


[SharpObject]
public partial class EditContainer : StackLayout, IEditContainerProperties
{
    public EditContainer()
    {
        Microsoft.Maui.Controls.AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.None);
        Microsoft.Maui.Controls.AbsoluteLayout.SetLayoutBounds(this, new Rect(0, 300, Microsoft.Maui.Controls.AbsoluteLayout.AutoSize, Microsoft.Maui.Controls.AbsoluteLayout.AutoSize));
        Loaded += OnLoaded;
    }

    private void OnLoaded(object? sender, EventArgs e)
    {
        if (ButtonHeight == 0) ButtonHeight = 60;
        if (ButtonWidth == 0) ButtonWidth = 60;

        _selectAreaButton = new($"Select area", SelectArea, OnSelectButtonChange, ButtonWidth, ButtonHeight);
        _rotateButton = new("Rotate", Rotate, OnSelectButtonChange, ButtonWidth, ButtonHeight);
        _mirrorButton = new("Mirror", Mirror, OnSelectButtonChange, ButtonWidth, ButtonHeight);
        _deleteButton = new("Delete", Delete, OnSelectButtonChange, ButtonWidth, ButtonHeight);
        Add(_selectAreaButton);
        Add(_rotateButton);
        Add(_mirrorButton);
        Add(_deleteButton);

    }

    private EditButton _selectAreaButton;

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

    private EditButton _deleteButton;
    private EditButton _mirrorButton;
    private EditButton _rotateButton;
    private EditButton? _lastButton;
}

