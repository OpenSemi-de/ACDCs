using System.Collections.ObjectModel;
using System.Reflection;
using ACDCs.ApplicationLogic.Components.Properties;
using ACDCs.ApplicationLogic.Components.Window;
using Microsoft.Maui.Layouts;
using UraniumUI.Material.Controls;

namespace ACDCs.ApplicationLogic.Views.Properties;

#pragma warning disable IDE0065

using Sharp.UI;

#pragma warning restore IDE0065

public class PropertiesView : WindowView, IGetPropertyUpdates
{
    public Action? OnUpdate;
    private readonly Grid _propertiesGrid;
    private readonly ScrollView _propertiesScroll;
    private readonly TreeView _propertiesView;
    private readonly ObservableCollection<PropertyItem> _roots;
    private object? _currentObject;
    public List<string> PropertyExcludeList { get; } = new();

    public PropertiesView(AbsoluteLayout? layout) : base(layout, "Properties")
    {
        _propertiesGrid = new Grid()
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill)
            .Padding(0)
            .Margin(0);

        _propertiesScroll = new ScrollView()
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill);

        Microsoft.Maui.Controls.DataTemplate itemTemplate = new();

        itemTemplate.LoadTemplate = () =>
        {
            PropertyTemplate propertyTemplate = new(this);
            propertyTemplate.Unloaded += (sender, args) =>
            {
                itemTemplate.Bindings.Clear();
            };
            return propertyTemplate;
        };

        _propertiesView = new TreeView
        {
            Spacing = 0,
            HorizontalOptions = LayoutOptions.Fill,
            IsExpandedPropertyName = "IsExpanded",
            IsLeafPropertyName = "IsLeaf",
            ItemTemplate = itemTemplate
        };

        _propertiesScroll.Content(_propertiesView);

        _propertiesGrid.Add(_propertiesScroll);

        _roots = new ObservableCollection<PropertyItem>();

        _propertiesView.ItemsSource = _roots;
        _propertiesView.ChildrenBinding.Mode = BindingMode.OneTime;
        WindowContent = _propertiesGrid;

        HideMenuButton();
        HideResizer();
        SizeChanged += PropertiesView_SizeChanged;
    }

    public void GetProperties(object? currentObject)
    {
        _roots.Clear();
        GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);
        GC.WaitForFullGCComplete(1000);
        _currentObject = currentObject;
        IEnumerable<PropertyInfo>? properties = currentObject?.GetType().GetRuntimeProperties();

        List<PropertyItem> propertyItems = new();
        if (properties != null)
        {
            foreach (PropertyInfo propertyInfo in properties.OrderBy(p => p.PropertyType.Name).ThenBy(p => p.Name))
            {
                if (PropertyExcludeList.Contains(propertyInfo.Name))
                {
                    continue;
                }

                PropertyItem item = new(propertyInfo.Name) { IsLeaf = true };

                object? value = propertyInfo.GetValue(currentObject, null);
                if (value != null)
                {
                    item.Value = value;
                }

                propertyItems.Add(item);
            }
        }

        PropertyItem defaultRoot = new("Values")
        {
            IsExpanded = true
        };
        PropertyItem modelRoot = new("Model")
        {
            IsExpanded = true
        };
        PropertyItem extRoot = new("Extended") { IsExpanded = true };

        RerootItem(propertyItems, "Value", defaultRoot);
        RerootItem(propertyItems, "Type", defaultRoot);
        RerootItem(propertyItems, "Model", modelRoot);
        RerootItem(propertyItems, "IsMirrored", defaultRoot);
        RerootItem(propertyItems, "Rotation", defaultRoot);
        propertyItems.ForEach(item => extRoot.Children.Add(item));

        _roots.Add(defaultRoot);
        _roots.Add(modelRoot);
        _roots.Add(extRoot);
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

    private static void RerootItem(List<PropertyItem> propertyItems, string name, PropertyItem newRoot)
    {
        PropertyItem? valueItem = propertyItems.FirstOrDefault(p => p.Name == name);
        if (valueItem == null)
        {
            return;
        }

        propertyItems.Remove(valueItem);
        newRoot.Children.Add(valueItem);
    }

    private void PropertiesView_SizeChanged(object? sender, EventArgs e)
    {
        WidthRequest = 200;
        HeightRequest = 500;
        Microsoft.Maui.Controls.AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.None);
        if (MainContainer != null)
        {
            Microsoft.Maui.Controls.AbsoluteLayout.SetLayoutBounds(this,
                new Rect(MainContainer.Width - 202, 60, 200, 500));
        }
    }
}

public interface IGetPropertyUpdates
{
    void OnPropertyUpdated(string? propertyName, object value);
}
