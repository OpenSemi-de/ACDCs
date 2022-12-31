using System.Collections.ObjectModel;
using System.Reflection;
using CommunityToolkit.Maui.Views;
using SpiceSharp.Components;
using SpiceSharp.Components.Bipolars;
using SpiceSharp.ParameterSets;

namespace ACDCs.Views
{
    public partial class ComponentsDetailPopup : Popup
    {
        public ComponentsDetailPopup()
        {
            InitializeComponent();
        }

        public void Load(ComponentPageModel model)
        {
            titleLabel.Text = model.Name;
            propertiesTreeView.IsExpandedPropertyName = "IsExpanded";
            propertiesTreeView.IsLeafPropertyName = "IsLeaf";

            propertiesTreeView.ItemsSource = ToItemSource(model);
        }

        private void CloseButton_OnClicked(object? sender, EventArgs e)
        {
            Close();
        }

        private Dictionary<string, string> GetParameters(ModelParameters parameters)
        {
            Dictionary<string, string> parametersdic = new();
            IParameterSet parameterSet = parameters;
            foreach (PropertyInfo info in parameterSet.GetType().GetProperties())
            {
                parametersdic.Add(info.Name, Convert.ToString(info.GetValue(parameterSet)));
            }

            return parametersdic;
        }

        private ObservableCollection<PropertyItem> ToItemSource(ComponentPageModel model)
        {
            PropertyItem root = new PropertyItem(model.Name) { IsExpanded = true };
            if (model.Model is BipolarJunctionTransistorModel bjt)
            {
                foreach (var modelParameter in GetParameters(bjt.Parameters))
                {
                    if (modelParameter.Value != "")
                    {
                        PropertyItem item = new(modelParameter.Key) { IsExpanded = true };
                        PropertyItem valueItem = new(modelParameter.Value);
                        item.Children.Add(valueItem);
                        root.Children.Add(item);
                    }
                }
            }
            return new ObservableCollection<PropertyItem> { root };
        }
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
