namespace ACDCs.ApplicationLogic.Components;

using System.Collections.ObjectModel;
using System.Reflection;
using CommunityToolkit.Maui.Views;
using ModelSelection;
using Properties;

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

    private static Dictionary<string, string?> GetParameters(ComponentViewModel model)
    {
        Dictionary<string, string?> parametersdic = new();
        object? parameterSet = model.Model;
        if (parameterSet == null)
        {
            return parametersdic;
        }

        foreach (PropertyInfo info in parameterSet.GetType().GetProperties())
        {
            parametersdic.Add(info.Name, Convert.ToString(info.GetValue(parameterSet)));
        }

        return parametersdic;
    }

    private static ObservableCollection<PropertyItem> ToItemSource(ComponentViewModel model)
    {
        PropertyItem root = new(model.Name) { IsExpanded = true };

        Dictionary<string, string?> parameters = GetParameters(model);

        foreach (KeyValuePair<string, string?> modelParameter in parameters)
        {
            if (modelParameter.Value == "")
            {
                continue;
            }

            PropertyItem item = new(modelParameter.Key) { IsExpanded = true };
            PropertyItem valueItem = new(modelParameter.Value);
            item.Children.Add(valueItem);
            root.Children.Add(item);
        }

        return new ObservableCollection<PropertyItem> { root };
    }

    private void CloseButton_OnClicked(object? sender, EventArgs e)
    {
        Close();
    }
}
