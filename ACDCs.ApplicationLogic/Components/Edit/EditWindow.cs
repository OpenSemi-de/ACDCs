﻿namespace ACDCs.ApplicationLogic.Components.Edit;

using Sharp.UI;
using Window;

public class EditWindow : Window
{
    private readonly WindowContainer? _layout;
    private StackLayout _buttonLayout = new();
    private EditButton? _deleteButton;
    private EditButton? _lastButton;
    private EditButton? _mirrorButton;
    private EditButton? _rotateButton;
    private EditButton? _selectAreaButton;

    private double ButtonHeight { get; set; }

    private StackLayout ButtonLayout => _buttonLayout;

    private double ButtonWidth { get; set; }

    public EditWindow(WindowContainer? layout) : base(layout, "Tools", "", false, GetView)
    {
        _layout = layout;
        Initialize();
    }

    private static void CallHandler(string command)
    {
        API.Call(() =>
        {
            API.Instance.Call(command);
            return Task.CompletedTask;
        }).Wait();
    }

    private static void Delete()
    {
        CallHandler("delete");
    }

    private static View GetView(Window arg)
    {
        if (arg is EditWindow edit)
        {
            return edit.ButtonLayout;
        }

        return new Label();
    }

    private static void Mirror()
    {
        CallHandler("mirror");
    }

    private static void Rotate()
    {
        CallHandler("rotate");
    }

    private static void SelectArea()
    {
        CallHandler("selectarea");
    }

    private void AddButtons()
    {
        if (ButtonHeight == 0) ButtonHeight = 60;
        if (ButtonWidth == 0) ButtonWidth = 60;

        _selectAreaButton = new EditButton("Select", SelectArea, OnSelectButtonChange, ButtonWidth, ButtonHeight, true);
        _rotateButton = new EditButton("Rotate", Rotate, OnSelectButtonChange, ButtonWidth, ButtonHeight);
        _mirrorButton = new EditButton("Mirror", Mirror, OnSelectButtonChange, ButtonWidth, ButtonHeight);
        _deleteButton = new EditButton("Delete", Delete, OnSelectButtonChange, ButtonWidth, ButtonHeight);
        _buttonLayout.Add(_selectAreaButton);
        _buttonLayout.Add(_rotateButton);
        _buttonLayout.Add(_mirrorButton);
        _buttonLayout.Add(_deleteButton);
    }

    private void Initialize()
    {
        WidthRequest = 64;
        _buttonLayout = _buttonLayout
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill);
        Start();

        _layout?.SetWindowSize(this, 50, 200);
        _layout?.SetWindowPosition(this, 4, 50);
        HideWindowButtons();
        HideResizer();
        AddButtons();
    }

    private void OnSelectButtonChange(EditButton editButton)
    {
        _lastButton?.Deselect();
        _lastButton = editButton;
    }
}