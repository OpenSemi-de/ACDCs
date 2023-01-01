using System.Collections.ObjectModel;
using ACDCs.Data.ACDCs.Components;
using ACDCs.IO.Spice;
using CommunityToolkit.Maui.Views;
using Sharp.UI;
using Button = Microsoft.Maui.Controls.Button;
using ContentPage = Microsoft.Maui.Controls.ContentPage;
using Shell = Microsoft.Maui.Controls.Shell;

namespace ACDCs.Views;

[BindableProperties]
public interface IDataLineProperties
{
    Dictionary<string, string> Data { get; set; }
}

public partial class ComponentsPage : ContentPage
{
    public ObservableCollection<ComponentPageModel> dataSource;

    public ComponentsPage()
    {
        InitializeComponent();
        dataSource = new();

        ComponentsGrid.ItemsSource = dataSource;
    }

    public async void ImportSpiceModels(string fileName)
    {
        string jsonData = await File.ReadAllTextAsync(fileName);

        SpiceReader spiceReader = new();
        List<IElectronicComponent> components = spiceReader.ReadComponents(jsonData);
        if (spiceReader.HasErrors)
        {
            if (spiceReader.Errors != null)
            {
                await Shell.Current.CurrentPage.DisplayAlert("Model import failed",
                    string.Join(':', spiceReader.Errors), "ok");
            }

            return;
        }

        dataSource.Clear();
        foreach (var component in components)
        {
            ComponentPageModel modelLine = new()
            {
                Name = component.Name,
                Type = component.GetType().Name,
            };

            modelLine.Model = component;

            switch (component)
            {
                case Bjt bjt:
                    modelLine.Value = bjt.TypeName;
                    modelLine.Model = bjt;
                    break;
            }

            dataSource.Add(modelLine);
        }
    }

    private void DetailsButton_OnClicked(object? sender, EventArgs e)
    {
        if (sender is Button button &&
            button.CommandParameter is int row)
        {
            var model = dataSource[row - 1];
            ComponentsDetailPopup popup = new();
            this.ShowPopup(popup);
            popup.Load(model);
        }
    }
}

public class ComponentPageModel
{
    public IElectronicComponent Model { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string Value { get; set; }
}
