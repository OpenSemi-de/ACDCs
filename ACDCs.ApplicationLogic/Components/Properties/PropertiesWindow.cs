namespace ACDCs.API.Core.Components.Properties;

using System.Collections.ObjectModel;
using System.Reflection;
using ACDCs.API.Interfaces;
using ACDCs.API.Windowing.Components.Window;
using Sharp.UI;
using UraniumUI.Material.Controls;
using DataTemplate = DataTemplate;

public class PropertiesWindow : Window, IGetPropertyUpdates
{
    public Action? OnUpdate;
    private readonly Grid _propertiesGrid;
    private readonly ScrollView _propertiesScroll;
    private readonly TreeView _propertiesView;
    private readonly ObservableCollection<PropertyItem> _roots;
    private object? _currentObject;
    public List<string> PropertyExcludeList { get; } = new();

    public PropertiesWindow(WindowContainer? layout) : base(layout, "Properties")
    {
        _propertiesGrid = new Grid()
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill)
            .Padding(0)
            .Margin(0);

        _propertiesScroll = new ScrollView()
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill);

        DataTemplate itemTemplate = new();

        itemTemplate.LoadTemplate = () =>
        {
            PropertyTemplate propertyTemplate = new(this);
            propertyTemplate.Unloaded += (_, _) =>
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
        Start();
        ChildLayout.Add(_propertiesGrid);
        OnClose = OnClosePropertiesWindow;

        HideWindowButtonsExceptClose();
        HideResizer();
        const int width = 176;
        const int height = 300;
        MainContainer?.SetWindowSize(this, width, height);
        MainContainer?.SetWindowPosition(this, 4, 450);
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

    private static void RerootItem(ICollection<PropertyItem> propertyItems, string name, PropertyItem newRoot)
    {
        PropertyItem? valueItem = propertyItems.FirstOrDefault(p => p.Name == name);
        if (valueItem == null)
        {
            return;
        }

        propertyItems.Remove(valueItem);
        newRoot.Children.Add(valueItem);
    }

    private bool OnClosePropertiesWindow()
    {
        Task.Run(() =>
        {
            this.FadeTo(0).Wait();
            IsVisible = false;
        });
        return false;
    }
}
