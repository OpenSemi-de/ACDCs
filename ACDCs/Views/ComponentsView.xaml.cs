using System.Reflection;
using ACDCs.Data.ACDCs.Components;
using ACDCs.IO.DB;
using ACDCs.IO.Spice;
using ACDCs.Views.Components;
using CommunityToolkit.Maui.Views;
using Sharp.UI;
using Button = Microsoft.Maui.Controls.Button;
using Shell = Microsoft.Maui.Controls.Shell;

namespace ACDCs.Views;

[BindableProperties]
public interface IDataLineProperties
{
    Dictionary<string, string> Data { get; set; }
}

public partial class ComponentsView : SharpAbsoluteLayout
{
    public List<ComponentPageModel> dataSource = new();
    private List<ComponentPageModel> _baseData = new();
    private string _category = "";

    public ComponentsView()
    {
        InitializeComponent();
        Loaded += OnLoaded;
        SizeChanged += OnSizeChanged;
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

        _baseData = dataSource;
        Reload();
    }

    public async void OnClose()
    {
        ComponentsGrid.BatchBegin();
        ComponentsGrid.ItemsSource = null;
        ComponentsGrid.ItemsSource = new List<string>();
        dataSource.Clear();
        _baseData?.Clear();
        ComponentsGrid.BatchCommit();

        await Task.Delay(10000);
    }

    private void CategoryPicker_OnSelectedIndexChanged(object? sender, EventArgs e)
    {
        _category = CategoryPicker.SelectedItem as string ?? string.Empty;
        KeywordEntry_OnTextChanged(keywordEntry, null);
    }

    private void DetailsButton_OnClicked(object? sender, EventArgs e)
    {
        if (sender is Button button &&
            button.CommandParameter is int row)
        {
            ComponentPageModel model = dataSource[row];
            ComponentsDetailPopup popup = new();
            if (App.Current?.MainPage != null)
            {
                App.Current.MainPage.ShowPopup(popup);
            }

            popup.Load(model);
        }
    }

    private void KeywordEntry_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        IEnumerable<ComponentPageModel> query = _baseData;
        if (_category != "")
        {
            query = query.Where(d => d.Type.ToLower().Contains(_category.ToLower()));
        }

        string keyword = keywordEntry.Text.ToLower();

        if (keyword != "")
        {
            query = query.Where(d => ReflectedSearch(d, keywordEntry.Text) ||
                                     d.Type.ToLower().Contains(keyword) ||
                                     d.Name.ToLower().Contains(keyword));
        }

        dataSource = query.ToList();
        Reload();
    }

    private void OnLoaded(object? sender, EventArgs e)
    {
        DBConnection defaultdb = new DBConnection("default");
        List<IElectronicComponent> defaultComponents = defaultdb.Read<IElectronicComponent>("Components");

        CategoryPicker.ItemsSource = dataSource
            .Select(cm => cm.Type).Distinct().ToList();
    }

    private void OnSizeChanged(object? sender, EventArgs e)
    {
        InvalidateMeasure();
        foreach (var child in this.Children)
        {
            child.InvalidateMeasure();
            child.InvalidateArrange();
        }
    }

    private bool ReflectedSearch(ComponentPageModel componentPageModel, string text)
    {
        Type modelType = componentPageModel.Model.GetType();
        text = text.ToLower();
        foreach (PropertyInfo propertyInfo in modelType.GetProperties())
        {
            string? value = Convert.ToString(propertyInfo.GetValue(componentPageModel.Model));
            if (value != null)
            {
                value = value.ToLower();
                if (value.Contains(text))
                    return true;
            }
            else
            {
                return false;
            }
        }

        return false;
    }

    private void Reload()
    {
        ComponentsGrid.BatchBegin();
        ComponentsGrid.ItemsSource = null;
        ComponentsGrid.ItemsSource = dataSource;
        ComponentsGrid.BatchCommit();
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
