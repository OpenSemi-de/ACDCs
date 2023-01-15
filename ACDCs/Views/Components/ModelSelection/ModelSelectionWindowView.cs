using System.Collections.ObjectModel;
using ACDCs.Data.ACDCs.Components;
using ACDCs.IO.DB;
using ACDCs.Services;
using ACDCs.Views.Components.Window;
using CommunityToolkit.Maui.Core.Extensions;

namespace ACDCs.Views.Components.ModelSelection;

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
    private ComponentViewModel? _lastSelectedItem;
    private IElectronicComponent? _selectedModel;

    public Action<IElectronicComponent> OnModelSelected { get; set; }

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
                break;
        }
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
            DBConnection defaultdb = new DBConnection("default");
            List<IElectronicComponent> defaultComponents = defaultdb.Read<IElectronicComponent>("Components");
            if (type == "NPN")
            {
                List<Bjt?> npns = defaultComponents
                    .Where(c => (c is Bjt bjt) && bjt.TypeName == "npn")
                    .Select(c => c as Bjt).ToList();
                _componentsList.ItemsSource = npns
                    .Select(c =>
                        new ComponentViewModel() { Model = c, Name = c.Name, Type = type, Value = "" }).ToList()
                    .ToObservableCollection();

                return Task.CompletedTask;
            }

            // _componentsList.ItemsSource = defaultComponents;

            _componentsList.ItemsSource = defaultComponents
                .Select(c =>
                    new ComponentViewModel()
                    {
                        Model = c,
                        Name = c.Name,
                        Type = type,
                        Value = ""
                    }).ToList().ToObservableCollection();

            return Task.CompletedTask;
        });
    }

    private void Model_Selected(object? sender, SelectedItemChangedEventArgs e)
    {
        ComponentViewModel? viewModel = e.SelectedItem as ComponentViewModel;
        if (viewModel != null)
        {
            SetItemBackground(viewModel);
            _selectedModel = viewModel.Model;
        }
    }

    private void OKButton_Click(object? sender, EventArgs e)
    {
        if (_selectedModel != null)
        {
            OnModelSelected(_selectedModel);
            IsVisible = false;
        }
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
}
