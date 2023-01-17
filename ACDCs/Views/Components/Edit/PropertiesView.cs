using System.Collections.ObjectModel;
using System.Reflection;
using ACDCs.Data.ACDCs.Components;
using ACDCs.Views.Components.Window;
using Microsoft.Maui.Layouts;

namespace ACDCs.Views.Components.Edit;

using Sharp.UI;

public class PropertiesView : WindowView
{
    private readonly Grid _propertiesGrid;
    private readonly ListView _propertiesView;
    private object? _currentObject;

    public Action<PropertyEditor>? OnModelSelectionClicked { get; set; }
    public Action<IElectronicComponent> OnModelSelectionForward { get; set; }
    public Action? OnUpdate { get; set; }
    public List<string> PropertyExcludeList { get; set; } = new();

    public PropertiesView(SharpAbsoluteLayout layout) : base(layout, "Properties")
    {
        _propertiesGrid = new Grid()
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill)
            .Padding(0)
            .Margin(0);

        _propertiesView = new ListView()
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill);

        DataTemplate itemTemplate = new()
        {
            LoadTemplate = () => new PropertyTemplate(OnPropertyUpdated, ModelSelectionClicked)
        };

        _propertiesView.ItemTemplate(itemTemplate);

        _propertiesGrid.Add(_propertiesView);

        WindowContent = _propertiesGrid;

        base.HideMenuButton();
        base.HideResizer();
        layout.SizeChanged += PropertiesView_SizeChanged;
    }

    public void GetProperties(object? currentObject)
    {
        _currentObject = currentObject;
        var properties = currentObject?.GetType().GetRuntimeProperties();

        ObservableCollection<PropertyItem> propertyItems = new();
        if (properties != null)
        {
            foreach (PropertyInfo propertyInfo in properties.OrderBy(p => p.PropertyType.Name).ThenBy(p => p.Name))
            {
                if (
                    !PropertyExcludeList.Contains(propertyInfo.Name)
                // && (propertyInfo.PropertyType.IsPrimitive || propertyInfo.PropertyType.IsEnum)
                )
                {
                    PropertyItem item = new() { Name = propertyInfo.Name };
                    object? value = propertyInfo.GetValue(currentObject, null);
                    if (value != null)
                    {
                        item.Value = value;
                    }

                    propertyItems.Add(item);
                }
            }
        }

        _propertiesView.ItemsSource(propertyItems);
    }

    public void OnModelSelected(IElectronicComponent obj)
    {
        OnModelSelectionForward(obj);
    }

    private void ModelSelectionClicked(PropertyEditor obj)
    {
        OnModelSelectionClicked?.Invoke(obj);
        OnModelSelectionForward = obj.OnModelSelected;
    }

    private void OnPropertyUpdated(string? propertyName, object value)
    {
        try
        {
            Type? currentType = _currentObject?.GetType();
            if (propertyName != null && currentType != null && Convert.ToString(value) != "")
            {
                PropertyInfo? propertyInfo = currentType.GetProperty(propertyName);
                if (propertyInfo != null)
                {
                    object? outputValue = value;

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
        catch (Exception exception)
        {
            //   API.PopupException(exception);
        }

        OnUpdate?.Invoke();
    }

    private void PropertiesView_SizeChanged(object? sender, EventArgs e)
    {
        if (MainContainer.Width < 150) return;
        Microsoft.Maui.Controls.AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.None);
        Microsoft.Maui.Controls.AbsoluteLayout.SetLayoutBounds(this, new Rect(MainContainer.Width - 152, 60, 148, 400));
    }
}
