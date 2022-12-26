using System.Collections.ObjectModel;
using ACDCs.Data.ACDCs.Components;
using ACDCs.IO.Spice;
using Newtonsoft.Json;
using SpiceSharp.Entities;

namespace ACDCs.Views;

public partial class ComponentsPage : ContentPage
{
    private readonly ObservableCollection<IElectronicComponent> _currentComponents = new();

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
        foreach (var m in models)
        {
            IElectronicComponent? component =
                JsonConvert.DeserializeObject<IElectronicComponent>(JsonConvert.SerializeObject(m));
            if (component != null)
            {
                _currentComponents.Add(component);
            }

            ComponentsGrid.ItemsSource = _currentComponents;
        }
    }
}
