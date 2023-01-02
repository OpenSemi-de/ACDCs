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
    public List<ComponentPageModel> dataSource = new();

    public ComponentsPage()
    {
        InitializeComponent();
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

            // return;
        }
        LoadFromSource(components);
    }

    public void LoadFromSource(List<IElectronicComponent> components)
    {
        dataSource.Clear();
        components = components.OrderBy(c => c.Name).ThenBy(c => c.Type).ToList();
        int row = 0;
        foreach (IElectronicComponent component in components)
        {
            ComponentPageModel modelLine = new()
            {
                Name = component.Name,
                Type = component.GetType().Name,
                Row = row,
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
            row++;
        }
        ComponentsGrid.ItemsSource = dataSource;
    }

    private void DetailsButton_OnClicked(object? sender, EventArgs e)
    {
        if (sender is Button button &&
            button.CommandParameter is int row)
        {
            ComponentPageModel model = dataSource[row];
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
    public int Row { get; set; }
    public string Type { get; set; }
    public string Value { get; set; }
}
