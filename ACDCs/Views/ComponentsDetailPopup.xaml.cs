using System.Collections.ObjectModel;
using System.Reflection;
using ACDCs.Views.Components.ModelSelection;
using CommunityToolkit.Maui.Views;

namespace ACDCs.Views;

public partial class ComponentsDetailPopup : Popup
{
    public ComponentsDetailPopup()
    {
        InitializeComponent();
    }

    public void Load(ComponentViewModel model)
    {
        propertiesTreeView.IsExpandedPropertyName = "IsExpanded";
        propertiesTreeView.IsLeafPropertyName = "IsLeaf";

        propertiesTreeView.ItemsSource = ToItemSource(model);
    }

    private void CloseButton_OnClicked(object? sender, EventArgs e)
    {
        Close();
    }

    private Dictionary<string, string> GetParameters(ComponentViewModel model)
    {
        Dictionary<string, string> parametersdic = new();
        object parameterSet = model.Model;
        foreach (PropertyInfo info in parameterSet.GetType().GetProperties())
        {
            parametersdic.Add(info.Name, Convert.ToString(info.GetValue(parameterSet)));
        }

        return parametersdic;
    }

    private ObservableCollection<PropertyItem> ToItemSource(ComponentViewModel model)
    {
        PropertyItem root = new PropertyItem(model.Name) { IsExpanded = true };

        Dictionary<string, string> parameters = GetParameters(model);

        foreach (KeyValuePair<string, string> modelParameter in parameters)
        {
            if (modelParameter.Value != "")
            {
                PropertyItem item = new(modelParameter.Key) { IsExpanded = true };
                PropertyItem valueItem = new(modelParameter.Value);
                item.Children.Add(valueItem);
                root.Children.Add(item);
            }
        }

        return new ObservableCollection<PropertyItem> { root };
    }
}

public class PropertyItem
{
    public virtual IList<PropertyItem> Children { get; set; } = new ObservableCollection<PropertyItem>();

    public virtual bool IsExpanded { get; set; }
    public virtual string Name { get; set; } = "";

    public PropertyItem()
    {
    }

    public PropertyItem(string name)
    {
        Name = name;
    }
}
