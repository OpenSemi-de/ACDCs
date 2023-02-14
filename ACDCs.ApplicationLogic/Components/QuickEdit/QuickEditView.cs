namespace ACDCs.ApplicationLogic.Components.QuickEdit;

using ACDCs.CircuitRenderer.Interfaces;
using ACDCs.CircuitRenderer.Items;
using Data.ACDCs.Interfaces;
using ModelSelection;
using Sharp.UI;
using Window;

public class QuickEditView : Grid
{
    private readonly Button _editButton;
    private readonly Button _modelButton;
    private readonly ModelSelectionWindow _modelSelectionWindow;
    private readonly Label _unitDescriptionLabel;
    private readonly Label _unitLabel;
    private readonly Entry _valueEntry;
    private readonly Window? _window;
    private IWorksheetItem? _currentItem;

    public QuickEditView(Window? window)
    {
        _window = window;
        this.Margin(2)
        .ColumnDefinitions(new ColumnDefinitionCollection
        {
            new ColumnDefinition(90),
            new ColumnDefinition(60),
            new ColumnDefinition(20),
            new ColumnDefinition(100),
            new ColumnDefinition(100),
            new ColumnDefinition()
        })
        .RowDefinitions(new RowDefinitionCollection
        {
            new RowDefinition(30)
        });

        _unitDescriptionLabel = new QuickEditLabel("Select item")
            .HorizontalTextAlignment(TextAlignment.End);
        Add(_unitDescriptionLabel);

        _valueEntry = new Entry("")
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill)
            .IsEnabled(true)
            .Margin(0)
            .Column(1);
        Add(_valueEntry);

        _unitLabel = new QuickEditLabel("-")
            .FontSize(20)
            .HorizontalTextAlignment(TextAlignment.Start)
            .Column(2);
        Add(_unitLabel);

        string text = "Select model";
        _modelButton = new Button()
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill)
            .Margin(0)
            .Padding(0)
            .Text(text)
            .OnClicked(SelectModelButton_Clicked)
            .Column(3);
        Add(_modelButton);

        _editButton = new Button("Edit model")
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill)
            .Margin(0)
            .Padding(0)
            .OnClicked(EditModelButton_Clicked)
            .Column(4);
        Add(_editButton);

        _modelSelectionWindow = new ModelSelectionWindow(_window?.MainContainer)
        {
            OnModelSelected = OnModelSelected,
            ZIndex = 10,
            OnClose = OnClose,
            IsVisible = false
        };
    }

    public void UpdateEditor(IWorksheetItem item)
    {
        _currentItem = item;
        _unitDescriptionLabel.Text = "";
        _unitLabel.Text = "";

        string typeName = item.GetType().Name.Replace("Item", "");
        _window?.SetTitle($"{typeName} / {item.RefName}");

        switch (item)
        {
            case ResistorItem:
                _unitDescriptionLabel.Text = "Resistance";
                _unitLabel.Text = "Ω";
                break;

            case InductorItem:
                _unitDescriptionLabel.Text = "Henry";
                _unitLabel.Text = "H";
                break;

            case CapacitorItem:
                _unitDescriptionLabel.Text = "Farad";
                _unitLabel.Text = "F";
                break;

            default:
                _unitLabel.Text = "";
                break;
        }
    }

    private void EditModelButton_Clicked(object? sender, EventArgs e)
    {
    }

    private bool OnClose()
    {
#pragma warning disable CS4014
        API.Call(async () =>
#pragma warning restore CS4014
        {
            await _modelSelectionWindow.FadeTo(0);
            _modelSelectionWindow.IsVisible = false;
        });

        return false;
    }

    private void OnModelSelected(IElectronicComponent component)
    {
        if (_currentItem is WorksheetItem item)
        {
            item.Model = component;
        }
    }

    private async void SelectModelButton_Clicked(object? sender, EventArgs e)
    {
        await API.Call(() =>
         {
             if (_currentItem == null)
             {
                 return Task.CompletedTask;
             }

             _modelSelectionWindow.IsVisible = true;
             _modelSelectionWindow.FadeTo(1);
             _modelSelectionWindow?.SetComponentType(_currentItem.GetType().Name);
             API.TabBar?.BringToFront(_modelSelectionWindow);
             return Task.CompletedTask;
         });
    }
}

public class QuickEditLabel : Label
{
    public QuickEditLabel(string text)
    {
        this.Text(text);
    }
}
