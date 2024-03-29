﻿namespace ACDCs.API.Core.Components.Edit;

using Instance;

public class EditView : Grid
{
    private EditButton? _deleteButton;
    private EditButton? _lastButton;
    private EditButton? _mirrorButton;
    private EditButton? _propertiesButton;
    private EditButton? _rotateButton;
    private EditButton? _selectAreaButton;

    public EditView()
    {
        RowDefinition[] rows =
        {
            new(),
            new(),
            new(),
            new(),
            new(),
        };
        this.RowDefinitions(rows);
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

    private static void ShowProperties()
    {
        CallHandler("showproperties");
    }

    private void AddButtons()
    {
        _selectAreaButton = new EditButton("Select", SelectArea, OnSelectButtonChange, null, 74, 50, true);
        _propertiesButton =
            new EditButton("Properties", ShowProperties, OnSelectButtonChange, null, 74, 50);
        _rotateButton = new EditButton("Rotate", Rotate, OnSelectButtonChange, null, 74, 50);
        _mirrorButton = new EditButton("Mirror", Mirror, OnSelectButtonChange, null, 74, 50);
        _deleteButton = new EditButton("Delete", Delete, OnSelectButtonChange, null, 74, 50);
        this.Add(_selectAreaButton, 0, 0);
        this.Add(_propertiesButton, 0, 1);
        this.Add(_rotateButton, 0, 2);
        this.Add(_mirrorButton, 0, 3);
        this.Add(_deleteButton, 0, 4);
    }

    private void Initialize()
    {
        this.Margin(2)
            .Padding(2)
            .RowSpacing(2)
            .ColumnSpacing(2);

        AddButtons();
    }

    private void OnSelectButtonChange(EditButton editButton)
    {
        _lastButton?.Deselect();
        _lastButton = editButton;
    }
}
