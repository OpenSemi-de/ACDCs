﻿namespace ACDCs.API.Core.Components.Debug;

using Microsoft.Maui.Layouts;
using UraniumUI.Material.Controls;
using Windowing.Components.Window;

public class DebugWindow : Window
{
    private readonly Button _button;
    private readonly Grid _debugGrid;
    private readonly Label _label;
    private readonly TextField _textField;

    public Workbench? StartCenterPage { get; set; }

    public DebugWindow(WindowContainer? layout) : base(layout, "Debug", "")
    {
        _debugGrid = new Grid()
            .ColumnDefinitions(new ColumnDefinitionCollection(
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(new GridLength(60))
            ))
            .RowDefinitions(new RowDefinitionCollection(
                new RowDefinition(GridLength.Star),
                new RowDefinition(new GridLength(40))
            ))
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill);

        AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.PositionProportional);
        AbsoluteLayout.SetLayoutBounds(this, new Rect(1, 1, 300, 400));

        _label = new Label("ACDCs Debug console" + Environment.NewLine)
            .MaxLines(int.MaxValue);
        Grid.SetColumnSpan(_label, 2);
        _textField = new TextField();
        _textField.TextChanged += TextFieldOnTextChanged;
        Grid.SetRow(_textField, 1);
        _debugGrid.Add(_label);
        _debugGrid.Add(_textField);
        _button = new Button(">")
            .WidthRequest(60);
        Grid.SetRow(_button, 1);
        Grid.SetColumn(_button, 1);
        _button.Clicked += ButtonOnClicked;
        _debugGrid.Add(_button);
        Start();
        ChildLayout.Add(_debugGrid);
    }

    public void Write(string text)
    {
        _label.Text = text;
    }

    private static void TextFieldOnTextChanged(object? sender, TextChangedEventArgs e)
    {
    }

    private void ButtonOnClicked(object? sender, EventArgs e)
    {
        try
        {/*
            dynamic script = CSScript.Evaluator
                .LoadCode(@$"
using ACDCs;
using ACDCs.Services;
using ACDCs.Views;
using ACDCs.Views.Components;
using ACDCs.Views.Components.CircuitView;
using ACDCs.Views.Components.Debug;
using ACDCs.Views.Components.Window;
using ACDCs.Views.Components.Menu;
using ACDCs.Views.Components.Menu.MenuHandlers;
using ACDCs.Data.ACDCs.Components.Resistor;
using ACDCs.Data.ACDCs.Components;
using System;

public class Script {{
public void Execute(DebugWindow dView)
{{
try{{
{_script}
}}catch(Exception ex)
{{dView.Write(ex.ToString());}}
}}
}}

");

            script.Execute(this);
        */
        }
        catch (Exception exception)
        {
            Write(exception.ToString());
        }
    }
}
