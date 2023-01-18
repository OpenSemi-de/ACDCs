using System.Collections.ObjectModel;
using ACDCs.Components;
using ACDCs.Data.ACDCs.Components;
using ACDCs.Data.ACDCs.Components.Resistor;
using ACDCs.IO.DB;
using ACDCs.Services;
using ACDCs.Views.Window;
using CommunityToolkit.Maui.Core.Extensions;

namespace ACDCs.Views.ModelSelection;

using ACDCs.Components.ModelSelection;
using Sharp.UI;

public class ModelSelectionWindowView : WindowView
{
    private readonly StackLayout _buttonStack;
    private readonly Button _cancelButton;
    private readonly ListView _componentsList;
    private readonly Label _dividerButtons;
    private readonly Grid _modelGrid;
    private readonly Button _okButton;
    private readonly StackLayout _pagingStack;
    private readonly Entry _searchEntry;
    private string? _componentType;
    private ObservableCollection<ComponentViewModel>? _fullCollection;
    private ComponentViewModel? _lastSelectedItem;
    private IElectronicComponent? _selectedModel;
    public Action<IElectronicComponent>? OnModelSelected { get; set; }

    public ModelSelectionWindowView(SharpAbsoluteLayout layout) : base(layout, "Select Model")
    {
        WidthRequest = 500;
        HeightRequest = 500;
        this.HideMenuButton();
        this.HideResizer();

        _modelGrid = new Grid()
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill)
            .Margin(0)
            .Padding(0);

        _modelGrid.RowDefinitions.Add(new(new(40)));
        _modelGrid.RowDefinitions.Add(new(GridLength.Star));
        _modelGrid.RowDefinitions.Add(new(new(40)));
        _modelGrid.RowDefinitions.Add(new(new(40)));

        _modelGrid.ColumnDefinitions.Add(new(GridLength.Star));

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

        _modelGrid.Add(_searchEntry);
        _modelGrid.Add(_componentsList);
        _modelGrid.Add(_pagingStack);
        _modelGrid.Add(_buttonStack);

        WindowContent = _modelGrid;
    }

    public void SetComponentType(string componentType)
    {
        _componentType = componentType;
        switch (_componentType)
        {
            case "Bjt:NPN":
                LoadDB("NPN");
                return;

            case "Bjt:PNP":
                LoadDB("PNP");
                return;
        }

        LoadDB(componentType);
    }

    private static string SourceName(IElectronicComponent c)
    {
        if (c is Resistor)
        {
            return ResistorCalculator.GetStringValue(c.Value);
        }

        return c.Name != "" ? c.Name : c.Value;
    }

    private void CancelButton_Clicked(object? sender, EventArgs e)
    {
        this.IsVisible = false;
    }

    private void ComponentsList_ItemTapped(object? sender, ItemTappedEventArgs e)
    {
        if (e.Item is ComponentViewModel selectedItem)
        {
            SetItemBackground(selectedItem);
            if (_componentsList.ItemsSource is ObservableCollection<ComponentViewModel> model)
            {
                model.Move(0, 0);
            }
        }
    }

    private async void LoadDB(string type)
    {
        await API.Call(() =>
        {
            SetItemSource(Array.Empty<IElectronicComponent>());
            DefaultModelRepository repository = new();

            if (type == "NPN" || type == "PNP")
            {
                List<Bjt> bjts = repository.GetModels<Bjt>(type.ToLower());
                SetItemSource(bjts);

                return Task.CompletedTask;
            }
            else
            if (type == "Resistor")
            {
                List<Resistor> resistors = repository.GetModels<Resistor>(type.ToLower());

                SetItemSource(resistors.Union(ResistorCalculator.GetAllValues()));
                return Task.CompletedTask;
            }
            else
            if (type == "Diode")
            {
                List<Diode> diodes = repository.GetModels<Diode>(type.ToLower());
                SetItemSource(diodes);
                return Task.CompletedTask;
            }

            //  SetItemSource(repository.GetModels().Union(ResistorCalculator.GetAllValues()));

            return Task.CompletedTask;
        });
    }

    private void Model_Selected(object? sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem is ComponentViewModel viewModel)
        {
            SetItemBackground(viewModel);
            _selectedModel = viewModel.Model;
        }
    }

    private void OKButton_Click(object? sender, EventArgs e)
    {
        if (_selectedModel != null)
        {
            OnModelSelected?.Invoke(_selectedModel);
            IsVisible = false;
        }
    }

    private void SearchTextChanged(object? sender, TextChangedEventArgs e)
    {
        _componentsList.ItemsSource(e.NewTextValue != ""
            ? _fullCollection?.Where(c => c.Name != null && c.Name.ToLower().Contains(e.NewTextValue))
            : _fullCollection);
    }

    private void SetItemBackground(ComponentViewModel selectedItem)
    {
        selectedItem.ItemBackground = ColorService.Border;
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
            Type = c.Type != "" ? c.Type :
                   (c is Bjt bjt ? bjt.TypeName : ""),
            Value = SourceDescription(c)
        }).ToObservableCollection();

        _componentsList.ItemsSource = _fullCollection;
    }

    private string SourceDescription(IElectronicComponent electronicComponent)
    {
        if (electronicComponent is Resistor resistor)
        {
            return $"Series: {resistor.Series}, tolerance: {resistor.Tolerance}%";
        }

        return electronicComponent.Name != "" ? electronicComponent.Name : electronicComponent.Value;
    }
}
