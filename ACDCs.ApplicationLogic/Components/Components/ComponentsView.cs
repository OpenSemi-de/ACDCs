namespace ACDCs.ApplicationLogic.Components;

using System.Reflection;
using ACDCs.Data.ACDCs.Components.BJT;
using ACDCs.Data.ACDCs.Interfaces;
using CommunityToolkit.Maui.Views;
using Components;
using IO.Spice;
using ModelSelection;
using Sharp.UI;

public class ComponentsView : Grid
{
    public List<ComponentViewModel> dataSource = new();
    private readonly string _category = string.Empty;
    private readonly ListView _componentsGrid;
    private readonly Entry _keywordEntry;
    private readonly Window.Window _window;
    private List<ComponentViewModel> _baseData = new();

    public ComponentsView(Window.Window window)
    {
        this.Margin(4);
        _window = window;

        ColumnDefinitionCollection columnDefinitions = new() { new Microsoft.Maui.Controls.ColumnDefinition() };
        this.ColumnDefinitions(columnDefinitions);

        RowDefinitionCollection rowDefinitions = new()
        {
            new Microsoft.Maui.Controls.RowDefinition(40),
            new Microsoft.Maui.Controls.RowDefinition(34), new Microsoft.Maui.Controls.RowDefinition()
        };

        this.RowDefinitions(rowDefinitions);

        _keywordEntry = new Entry()
            .OnTextChanged(KeywordEntry_OnTextChanged)
            .HorizontalOptions(LayoutOptions.Fill);

        StackLayout inputlayout = new() { new Label("Search text:"), _keywordEntry };
        inputlayout.Row(1).Orientation(StackOrientation.Horizontal);
        Add(inputlayout);

        _componentsGrid = new ListView()
            .ItemTemplate(new ComponentsGridTemplate())
            .Row(2);

        Add(_componentsGrid);

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

    // ReSharper disable once UnusedMember.Global
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

    private static bool ReflectedSearch(ComponentViewModel componentViewModel, string text)
    {
        Type? modelType = componentViewModel.Model?.GetType();
        text = text.ToLower();
        if (modelType == null)
        {
            return false;
        }

        foreach (PropertyInfo propertyInfo in modelType.GetProperties())
        {
            string? value = Convert.ToString(propertyInfo.GetValue(componentViewModel.Model));
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

    private void DetailsButton_OnClicked(object? sender, EventArgs e)
    {
        if (sender is not Button { CommandParameter: int row })
        {
            return;
        }

        ComponentViewModel model = dataSource[row];
        ComponentsDetailPopup popup = new();
        if (API.MainPage != null)
        {
            API.MainPage.ShowPopup(popup);
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

        string keyword = _keywordEntry.Text.ToLower();

        if (keyword != "")
        {
            query = query.Where(d => d.Name != null && (ReflectedSearch(d, _keywordEntry.Text) ||
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
    }

    private void OnSizeChanged(object? sender, EventArgs e)
    {
        InvalidateMeasure();
        foreach (Microsoft.Maui.IView? child in Children)
        {
            child.InvalidateMeasure();
            child.InvalidateArrange();
        }
    }

    private void Reload()
    {
        _componentsGrid.ItemsSource = null;
        _componentsGrid.ItemsSource = dataSource;
    }
}
