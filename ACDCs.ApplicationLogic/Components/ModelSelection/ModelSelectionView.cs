namespace ACDCs.API.Core.Components.ModelSelection;

using System.Collections.ObjectModel;
using ACDCs.API.Windowing.Components.Window;
using ACDCs.CircuitRenderer.Items.Transistors;
using ACDCs.Data.ACDCs.Components.BJT;
using ACDCs.Data.ACDCs.Components.Diode;
using ACDCs.Data.ACDCs.Components.Resistor;
using ACDCs.Data.ACDCs.Interfaces;
using ACDCs.IO.DB;
using CommunityToolkit.Maui.Core.Extensions;
using Instance;
using Sharp.UI;
using ColumnDefinition = ColumnDefinition;
using RowDefinition = RowDefinition;

public class ModelSelectionView : Grid
{
    private readonly StackLayout _buttonStack;
    private readonly Button _cancelButton;
    private readonly ListView _componentsList;
    private readonly Label _dividerButtons;
    private readonly Button _okButton;
    private readonly StackLayout _pagingStack;
    private readonly Entry _searchEntry;
    private readonly Window _window;
    private string? _componentType;
    private ObservableCollection<ComponentViewModel>? _fullCollection;
    private ComponentViewModel? _lastSelectedItem;
    private IElectronicComponent? _selectedModel;
    public Action<IElectronicComponent>? OnModelSelected { get; set; }

    public ModelSelectionView(Window window)
    {
        _window = window;

        this.HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill)
            .Margin(0)
            .Padding(0);

        RowDefinitions.Add(new RowDefinition(new GridLength(40)));
        RowDefinitions.Add(new RowDefinition(GridLength.Star));
        RowDefinitions.Add(new RowDefinition(new GridLength(40)));
        RowDefinitions.Add(new RowDefinition(new GridLength(40)));

        ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));

        _searchEntry = new Entry()
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill);

        _searchEntry.OnTextChanged(SearchTextChanged);

        _componentsList = new ListView()
            .OnItemTapped(ComponentsList_ItemTapped)
            .SelectionMode(ListViewSelectionMode.Single)
            .OnItemSelected(Model_Selected)
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill);
        Grid.SetRow(_componentsList, 1);

        _componentsList.ItemTemplate = new DataTemplate(() => new ModelSelectionCell());

        _pagingStack = new StackLayout()
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill)
            .Orientation(StackOrientation.Horizontal);
        Grid.SetRow(_pagingStack, 2);

        _buttonStack = new StackLayout()
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill)
            .Orientation(StackOrientation.Horizontal);
        Grid.SetRow(_buttonStack, 3);

        _cancelButton = new Button("Cancel")
            .HorizontalOptions(LayoutOptions.Start)
            .VerticalOptions(LayoutOptions.Fill)
            .WidthRequest(80)
            .OnClicked(CancelButton_Clicked);

        _buttonStack.Add(_cancelButton);

        _dividerButtons = new Label()
            .HorizontalOptions(LayoutOptions.Fill);
        _buttonStack.Add(_dividerButtons);

        _okButton = new Button("OK")
            .OnClicked(OKButton_Click)
            .HorizontalOptions(LayoutOptions.End)
            .VerticalOptions(LayoutOptions.Fill)
            .WidthRequest(80);

        _buttonStack.Add(_okButton);

        Add(_searchEntry);
        Add(_componentsList);
        Add(_pagingStack);
        Add(_buttonStack);
    }

    public void SetComponentType(string componentType)
    {
        _componentType = componentType;
        switch (_componentType)
        {
            case "Bjt:NPN":
            case nameof(NpnTransistorItem):
                LoadDB("NPN");
                return;

            case "Bjt:PNP":
            case nameof(PnpTransistorItem):
                LoadDB("PNP");
                return;

            default:
                LoadDB(_componentType);
                return;
        }
    }

    private static string SourceDescription(IElectronicComponent electronicComponent)
    {
        if (electronicComponent is Resistor resistor)
        {
            return $"Series: {resistor.Series}, tolerance: {resistor.Tolerance}%";
        }

        return electronicComponent.Name != "" ? electronicComponent.Name : electronicComponent.Value;
    }

    private static string SourceName(IElectronicComponent c)
    {
        if (c is Resistor && double.TryParse(c.Value, out double resValue))
        {
            return ResistorCalculator.GetStringValue(resValue);
        }

        return c.Name != "" ? c.Name : c.Value;
    }

    private static string SourceType(IElectronicComponent model)
    {
        return model.Type != "" ? model.Type : model is Bjt bjt ? bjt.TypeName : "";
    }

    private void CancelButton_Clicked(object? sender, EventArgs e)
    {
        _window.Close();
    }

    private void ComponentsList_ItemTapped(object? sender, ItemTappedEventArgs e)
    {
        if (e.Item is not ComponentViewModel selectedItem)
        {
            return;
        }

        SetItemBackground(selectedItem);

        if (_componentsList.ItemsSource is ObservableCollection<ComponentViewModel> model)
        {
            model.Move(0, 0);
        }
    }

    private async void LoadDB(string type)
    {
        type = type.Replace("Item", "");
        await API.Call(() =>
        {
            SetItemSource(Array.Empty<IElectronicComponent>());
            DefaultModelRepository repository = new();

            switch (type)
            {
                case "NPN":
                case "PNP":
                    {
                        List<Bjt> bjts = repository.GetModels<Bjt>(type.ToLower());
                        SetItemSource(bjts);
                        return Task.CompletedTask;
                    }
                case "Resistor":
                    {
                        List<Resistor> resistors = repository.GetModels<Resistor>(type.ToLower());
                        SetItemSource(resistors.Union(ResistorCalculator.GetAllValues()));
                        return Task.CompletedTask;
                    }
                case "Diode":
                    {
                        List<Diode> diodes = repository.GetModels<Diode>(type.ToLower());
                        SetItemSource(diodes);
                        return Task.CompletedTask;
                    }
                default:
                    //  SetItemSource(repository.GetModels().Union(ResistorCalculator.GetAllValues()));

                    return Task.CompletedTask;
            }
        });
    }

    private void Model_Selected(object? sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem is not ComponentViewModel viewModel)
        {
            return;
        }

        SetItemBackground(viewModel);
        _selectedModel = viewModel.Model;
    }

    private void OKButton_Click(object? sender, EventArgs e)
    {
        if (_selectedModel == null)
        {
            return;
        }

        OnModelSelected?.Invoke(_selectedModel);
        _window.Close();
    }

    private void SearchTextChanged(object? sender, TextChangedEventArgs e)
    {
        _componentsList.ItemsSource(e.NewTextValue != ""
            ? _fullCollection?.Where(c => c.Name != null && c.Name.ToLower().Contains(e.NewTextValue))
            : _fullCollection);
    }

    private void SetItemBackground(ComponentViewModel selectedItem)
    {
        selectedItem.ItemBackground = API.Instance.Border;
        if (_lastSelectedItem != null)
        {
            _lastSelectedItem.ItemBackground = Colors.Transparent;
        }

        _lastSelectedItem = selectedItem;
    }

    private void SetItemSource(IEnumerable<IElectronicComponent> componentViewModels)
    {
        _fullCollection = componentViewModels.Select(c =>
            new ComponentViewModel
            {
                Model = c,
                Name = SourceName(c),
                Type = SourceType(c),
                Value = SourceDescription(c)
            }).ToObservableCollection();

        _componentsList.ItemsSource = _fullCollection;
    }
}
