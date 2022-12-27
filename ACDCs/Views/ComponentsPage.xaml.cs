using System.Collections.ObjectModel;
using System.Reflection;
using ACDCs.IO.Spice;
using SpiceSharp.Components;
using SpiceSharp.Entities;

namespace ACDCs.Views;

public partial class ComponentsPage : ContentPage
{
    private readonly ObservableCollection<ComponentPageModel> _currentComponents = new();

    public ComponentsPage()
    {
        InitializeComponent();
        ComponentsGrid.ItemsSource = _currentComponents;
    }

    public async void ImportSpiceModels(string fileName)
    {
        string jsonData = await File.ReadAllTextAsync(fileName);

        SpiceReader spiceReader = new();
        List<IEntity> models = spiceReader.ReadModels(jsonData);
        if (spiceReader.HasErrors)
        {
            if (spiceReader.Errors != null)
            {
                await Shell.Current.CurrentPage.DisplayAlert("Model import failed",
                    string.Join(':', spiceReader.Errors), "ok");
            }

            return;
        }

        _currentComponents.Clear();
        foreach (IEntity entity in models)
        {
            switch (entity)
            {
                case BipolarJunctionTransistorModel model:
                    {
                        Dictionary<string, string> parameters = ObjectToDictionary(model.Parameters);
                        ComponentPageModel viewmodel = new()
                        {
                            Parameters = string.Join("", parameters).Replace("[", "").Replace("]", "<br />"),
                            Name = model.Name
                        };
                        _currentComponents.Add(viewmodel);
                        break;
                    }
            }
        }
    }

    private Dictionary<string, string> ObjectToDictionary(object modelObject)
    {
        Dictionary<string, string> values = new();
        var properties = modelObject.GetType().GetProperties().Where(prop => prop.DeclaringType == modelObject.GetType() && prop.PropertyType.IsPrimitive && prop.PropertyType.IsValueType);
        foreach (PropertyInfo propertyInfo in properties)
        {
            string name = propertyInfo.Name;
            string? value = Convert.ToString(propertyInfo.GetValue(modelObject));
            if (value != null)
            {
                values.Add(name, value);
            }
        }

        return values;
    }
}

public class ComponentPageModel
{
    public string Name { get; set; }
    public string Parameters { get; set; }
}
