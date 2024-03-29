﻿namespace ACDCs.API.Core.Components.ModelEditor;

using System.Collections.ObjectModel;
using System.Reflection;
using CommunityToolkit.Maui.Core.Extensions;
using Data.ACDCs.Interfaces;
using Instance;
using Interfaces;
using Properties;
using Windowing.Components.Window;

public class ModelEditorView : ContentView, IGetPropertyUpdates
{
    private readonly StackLayout _buttonStack;
    private readonly Button _cancelButton;
    private readonly Label _dividerButtons;
    private readonly Grid _modelGrid;
    private readonly ListView _modelView;
    private readonly Button _okButton;
    private readonly Window _window;
    private object? _currentObject;
    public Action<IElectronicComponent>? OnModelEdited { get; set; }

    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public Action? OnUpdate { get; set; }

    private List<string> PropertyExcludeList { get; } = new();

    public ModelEditorView(Window window)
    {
        _window = window;
        _modelGrid = new Grid()
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill)
            .Padding(2)
            .Margin(2);

        RowDefinitionCollection rows = new()
        {
            new RowDefinition(),
            new RowDefinition(34)
        };

        _modelGrid.RowDefinitions(rows);

        _modelView = new ListView()
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill);

        DataTemplate itemTemplate = new()
        {
            LoadTemplate = () => new ModelPropertyTemplate(this)
        };

        _modelView.ItemTemplate(itemTemplate);

        _modelGrid.Add(_modelView);

        _buttonStack = new StackLayout()
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill)
            .Orientation(StackOrientation.Horizontal);
        Grid.SetRow(_buttonStack, 3);

        _cancelButton = new Button("Cancel")
            .HorizontalOptions(LayoutOptions.Start)
            .VerticalOptions(LayoutOptions.Fill)
            .WidthRequest(80)
            .OnClicked(CancelButton_Clicked);

        _buttonStack.Add(_cancelButton);

        _dividerButtons = new Label()
            .HorizontalOptions(LayoutOptions.Fill);
        _buttonStack.Add(_dividerButtons);

        _okButton = new Button("OK")
            .OnClicked(OKButton_Click)
            .HorizontalOptions(LayoutOptions.End)
            .VerticalOptions(LayoutOptions.Fill)
            .WidthRequest(80);

        _buttonStack.Add(_okButton);
        Grid.SetRow(_buttonStack, 1);

        _modelGrid.Add(_buttonStack);
        // Start();
        Content = _modelGrid;
        //ChildLayout.Add(_modelGrid);
    }

    public void GetProperties(object? currentObject)
    {
        _currentObject = currentObject;
        Type? parentType = currentObject?.GetType();
        IEnumerable<PropertyInfo>? properties = currentObject?.GetType().GetRuntimeProperties();

        ObservableCollection<PropertyItem> propertyItems = new();
        if (properties != null)
        {
            foreach (PropertyInfo propertyInfo in properties.OrderBy(p => p.PropertyType.Name).ThenBy(p => p.Name))
            {
                if (PropertyExcludeList.Contains(propertyInfo.Name))
                {
                    continue;
                }

                // && (propertyInfo.PropertyType.IsPrimitive || propertyInfo.PropertyType.IsEnum)
                PropertyItem item = new(propertyInfo.Name);
                object? value = propertyInfo.GetValue(currentObject, null);
                if (value != null)
                {
                    item.Value = value;
                }

                if (parentType != null)
                {
                    item.Order = API.Instance.GetComponentPropertyOrder(parentType, propertyInfo.Name);
                    item.ParentType = parentType;
                }

                propertyItems.Add(item);
            }
        }

        propertyItems = propertyItems.OrderBy(item => item.Order).ToObservableCollection();
        _modelView.ItemsSource(propertyItems);
    }

    public void OnPropertyUpdated(string? propertyName, object value)
    {
        try
        {
            Type? currentType = _currentObject?.GetType();
            if (propertyName != null && currentType != null && Convert.ToString(value) != "")
            {
                PropertyInfo? propertyInfo = currentType.GetProperty(propertyName);
                if (propertyInfo != null)
                {
                    object outputValue = value;

                    if (propertyInfo.PropertyType == typeof(float))
                    {
                        outputValue = Convert.ToSingle(value);
                    }

                    if (propertyInfo.PropertyType == typeof(int))
                    {
                        outputValue = Convert.ToInt32(value);
                    }

                    if (propertyInfo.GetValue(_currentObject) != value)
                        propertyInfo.SetValue(_currentObject, outputValue);
                }
            }
        }
        catch (Exception)
        {
            //   API.PopupException(exception);
        }

        OnUpdate?.Invoke();
    }

    private void CancelButton_Clicked(object? sender, EventArgs e)
    {
        _window.Close();
    }

    private void OKButton_Click(object? sender, EventArgs e)
    {
        if (_currentObject != null)
        {
            OnModelEdited?.Invoke((IElectronicComponent)_currentObject);
        }

        _window.Close();
    }
}
