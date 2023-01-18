using ACDCs.Components;
using ACDCs.Components.Edit;
using ACDCs.Interfaces;
using ACDCs.Services;
using ACDCs.Views.Window;
using Microsoft.Maui.Layouts;

namespace ACDCs.Views.Edit;

using Sharp.UI;

[SharpObject]
public partial class EditView : WindowView, IEditViewProperties
{
    private StackLayout _buttonLayout = null!;
    private EditButton? _deleteButton;
    private EditButton? _lastButton;
    private EditButton? _mirrorButton;
    private EditButton? _rotateButton;
    private EditButton? _selectAreaButton;

    public EditView(SharpAbsoluteLayout layout) : base(layout, "Tools")
    {
        Initialize();
    }

    private static void CallHandler(string command)
    {
        API.Call(() =>
        {
            MenuService.Call(command);
            return Task.CompletedTask;
        }).Wait();
    }

    private void AddButtons()
    {
        if (ButtonHeight == 0) ButtonHeight = 60;
        if (ButtonWidth == 0) ButtonWidth = 60;

        _selectAreaButton = new EditButton("Select area", SelectArea, OnSelectButtonChange, ButtonWidth, ButtonHeight, true);
        _rotateButton = new EditButton("Rotate", Rotate, OnSelectButtonChange, ButtonWidth, ButtonHeight);
        _mirrorButton = new EditButton("Mirror", Mirror, OnSelectButtonChange, ButtonWidth, ButtonHeight);
        _deleteButton = new EditButton("Delete", Delete, OnSelectButtonChange, ButtonWidth, ButtonHeight);
        _buttonLayout.Add(_selectAreaButton);
        _buttonLayout.Add(_rotateButton);
        _buttonLayout.Add(_mirrorButton);
        _buttonLayout.Add(_deleteButton);
    }

    private void Delete()
    {
        CallHandler("delete");
    }

    private void Initialize()
    {
        Microsoft.Maui.Controls.AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.None);
        Microsoft.Maui.Controls.AbsoluteLayout.SetLayoutBounds(this, new Rect(0, 60, 68, AbsoluteLayout.AutoSize));

        _buttonLayout = new StackLayout()
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill);

        WindowContent = _buttonLayout;
        HideMenuButton();
        HideResizer();
        AddButtons();
    }

    private void Mirror()
    {
        CallHandler("mirror");
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
