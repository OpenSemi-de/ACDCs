using System.Reflection;
using ACDCs.Components;
using ACDCs.Data.ACDCs.Components;
using ACDCs.IO.Spice;
using CommunityToolkit.Maui.Views;

namespace ACDCs.Views;

using ACDCs.Components.ModelSelection;
using Sharp.UI;

public partial class ComponentsView : SharpAbsoluteLayout
{
    public List<ComponentViewModel> dataSource = new();
    private List<ComponentViewModel> _baseData = new();
    private string _category = string.Empty;

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
        if (spiceReader.HasErrors && spiceReader.Errors != null && API.MainPage != null)
        {
            await API.MainPage.DisplayAlert("Model import failed",
                string.Join(':', spiceReader.Errors), "ok");
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
            ComponentViewModel modelLine = new()
            {
                Name = component.Name,
                Type = component.GetType().Name,
                Row = row,
                Model = component
            };

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

        _baseData = dataSource.ToList();
        Reload();
    }

    public bool OnClose()
    {
        return true;
        /*
                ComponentsGrid.BatchBegin();
                dataSource.Clear();
                _baseData?.Clear();
                ComponentsGrid.ItemsSource = new List<string>();
                ComponentsGrid.BatchCommit();
        */
    }

    private static bool ReflectedSearch(ComponentViewModel ComponentViewModel, string text)
    {
        Type? modelType = ComponentViewModel.Model?.GetType();
        text = text.ToLower();
        if (modelType == null)
        {
            return false;
        }

        foreach (PropertyInfo propertyInfo in modelType.GetProperties())
        {
            string? value = Convert.ToString(propertyInfo.GetValue(ComponentViewModel.Model));
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

    private void CategoryPicker_OnSelectedIndexChanged(object? sender, EventArgs e)
    {
        _category = CategoryPicker.SelectedItem as string ?? string.Empty;
        KeywordEntry_OnTextChanged(keywordEntry, new TextChangedEventArgs(keywordEntry.Text, keywordEntry.Text));
    }

    private void DetailsButton_OnClicked(object? sender, EventArgs e)
    {
        if (sender is not Button button ||
            button.CommandParameter is not int row)
        {
            return;
        }

        ComponentViewModel model = dataSource[row];
        ComponentsDetailPopup popup = new();
        if (App.Current?.MainPage != null)
        {
            App.Current.MainPage.ShowPopup(popup);
        }

        popup.Load(model);
    }

    private void KeywordEntry_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        IEnumerable<ComponentViewModel> query = _baseData;
        if (_category != "")
        {
            query = query.Where(d => d.Type.ToLower().Contains(_category.ToLower()));
        }

        string keyword = keywordEntry.Text.ToLower();

        if (keyword != "")
        {
            query = query.Where(d => d.Name != null && (ReflectedSearch(d, keywordEntry.Text) ||
                                                        d.Type.ToLower().Contains(keyword) ||
                                                        d.Name.ToLower().Contains(keyword)));
        }

        dataSource.Clear();
        foreach (ComponentViewModel model in query)
        {
            dataSource.Add(model);
        }

        Reload();
    }

    private void OnLoaded(object? sender, EventArgs e)
    {
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

    private void Reload()
    {
        ComponentsGrid.ItemsSource = null;
        ComponentsGrid.ItemsSource = dataSource;
    }
}
