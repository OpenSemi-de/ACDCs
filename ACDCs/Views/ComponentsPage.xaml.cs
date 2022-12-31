using System.Collections.ObjectModel;
using ACDCs.IO.Spice;
using CommunityToolkit.Maui.Views;
using Sharp.UI;
using SpiceSharp.Components;
using SpiceSharp.Entities;
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

        dataSource.Clear();
        foreach (IEntity model in models)
        {
            ComponentPageModel modelLine = new()
            {
                Name = model.Name,
                Type = model.GetType().Name.Replace("Model", "")
            };
            modelLine.Model = model;
            switch (model)
            {
                case BipolarJunctionTransistorModel bjt:
                    modelLine.Value = bjt.Parameters.TypeName;
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
    public IEntity Model { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string Value { get; set; }
}
