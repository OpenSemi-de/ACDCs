using ACDCs.Views.Components.CircuitView;
using ACDCs.Views.Components.Menu.MenuHandlers;
using Microsoft.Maui.Layouts;

namespace ACDCs.Views.Components.Edit;

using Sharp.UI;

[BindableProperties]
public interface IEditContainerProperties
{
    double ButtonHeight { get; set; }
    double ButtonWidth { get; set; }
    CircuitViewContainer CircuitView { get; set; }
}

[SharpObject]
public partial class EditContainer : StackLayout, IEditContainerProperties
{
    private EditButton _deleteButton;

    private EditButton? _lastButton;

    private EditButton _mirrorButton;

    private EditButton _rotateButton;

    private EditButton _selectAreaButton;

    public EditContainer()
    {
        Microsoft.Maui.Controls.AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.None);
        Microsoft.Maui.Controls.AbsoluteLayout.SetLayoutBounds(this, new Rect(0, 300, Microsoft.Maui.Controls.AbsoluteLayout.AutoSize, Microsoft.Maui.Controls.AbsoluteLayout.AutoSize));
        Loaded += OnLoaded;
    }

    private static void CallHandler(string command)
    {
        App.Call(() =>
        {
            MenuHandler.Call(command);
            return Task.CompletedTask;
        }).Wait();
    }

    private void Delete()
    {
        CallHandler("delete");
    }

    private void Mirror()
    {
        CallHandler("mirror");
    }

    private void OnLoaded(object? sender, EventArgs e)
    {
        if (ButtonHeight == 0) ButtonHeight = 60;
        if (ButtonWidth == 0) ButtonWidth = 60;

        _selectAreaButton = new($"Select area", SelectArea, OnSelectButtonChange, ButtonWidth, ButtonHeight, true);
        _rotateButton = new("Rotate", Rotate, OnSelectButtonChange, ButtonWidth, ButtonHeight);
        _mirrorButton = new("Mirror", Mirror, OnSelectButtonChange, ButtonWidth, ButtonHeight);
        _deleteButton = new("Delete", Delete, OnSelectButtonChange, ButtonWidth, ButtonHeight);
        Add(_selectAreaButton);
        Add(_rotateButton);
        Add(_mirrorButton);
        Add(_deleteButton);
    }

    private void OnSelectButtonChange(EditButton editButton)
    {
        _lastButton?.Deselect();
        _lastButton = editButton;
    }

    private void Rotate()
    {
        CallHandler("rotate");
    }

    private void SelectArea()
    {
        CallHandler("selectarea");
    }
}
