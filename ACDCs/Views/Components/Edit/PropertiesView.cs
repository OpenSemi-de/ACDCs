using System.Collections.ObjectModel;
using System.Reflection;
using ACDCs.Views.Components.Window;
using Microsoft.Maui.Layouts;

namespace ACDCs.Views.Components.Edit;

using Sharp.UI;

public class PropertiesView : WindowView
{
    private readonly Grid _propertiesGrid;
    private readonly ListView _propertiesView;

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
            LoadTemplate = () => new PropertyTemplate()
        };

        _propertiesView.ItemTemplate(itemTemplate);

        _propertiesGrid.Add(_propertiesView);

        WindowContent = _propertiesGrid;

        base.HideMenuButton();
        base.HideResizer();
        layout.SizeChanged += PropertiesView_SizeChanged;
    }

    public void GetProperties(object? obj)
    {
        var properties = obj?.GetType().GetProperties();

        ObservableCollection<PropertyItem> propertyItems = new();
        foreach (PropertyInfo propertyInfo in properties)
        {
            if (propertyInfo.PropertyType.IsPrimitive)
            {
                PropertyItem item = new PropertyItem();
                item.Name = propertyInfo.Name;
                object? value = propertyInfo.GetValue(obj, null);
                if (value != null)
                {
                    item.Value = value;
                }

                propertyItems.Add(item);
            }
        }

        _propertiesView.ItemsSource(propertyItems);
    }

    private void PropertiesView_SizeChanged(object? sender, EventArgs e)
    {
        if (MainContainer.Width < 150) return;
        Microsoft.Maui.Controls.AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.None);
        Microsoft.Maui.Controls.AbsoluteLayout.SetLayoutBounds(this, new Rect(MainContainer.Width - 152, 60, 148, 400));
    }
}

[SharpObject()]
public partial class PropertyEditor : ContentView, IPropertyEditorProperties
{
    private int _fontSize;

    public PropertyEditor()
    {
        PropertyChanged += PropertyEditor_PropertyChanged;
    }

    public PropertyEditor Fontsize(int fontSize)
    {
        _fontSize = fontSize;
        return this;
    }

    private void PropertyEditor_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName != null && e.PropertyName.StartsWith("Value"))
        {
            var value = GetValue(ValueProperty);

            if (value is bool boolValue)
            {
                Content = new Switch()
                    .HorizontalOptions(LayoutOptions.Start)
                    .VerticalOptions(LayoutOptions.Start)
                    .IsToggled(boolValue);
            }
            else
            {
                Content = new Entry()
                    .HorizontalOptions(LayoutOptions.Fill)
                    .VerticalOptions(LayoutOptions.Fill)
                    .FontSize(_fontSize)
                    .Text(Convert.ToString(value));
            }
        }
    }
}

public class PropertyTemplate : ViewCell
{
    public PropertyTemplate()
    {
        Grid grid = new Grid()
            .HorizontalOptions(LayoutOptions.Fill)
            .HeightRequest(26);
        grid.ColumnDefinitions.Add(new(new(60)));
        grid.ColumnDefinitions.Add(new(GridLength.Star));

        Label label = new Label()
            .FontSize(10);

        label.SetBinding(Label.TextProperty, "Name");
        grid.Add(label);

        PropertyEditor entry = new PropertyEditor()
            .Fontsize(10);

        entry.SetBinding(PropertyEditor.ValueProperty, "Value");

        grid.Add(entry);
        Grid.SetColumn(entry, 1);
        this.Add(grid);
    }
}

[BindableProperties]
public interface IPropertyEditorProperties
{
    object Value { get; set; }
}
