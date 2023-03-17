// ReSharper disable StringLiteralTypo
namespace ACDCs.API.Core.Components.QuickEdit;

using ACDCs.CircuitRenderer.Items.Sources;
using ACDCs.CircuitRenderer.Items.Transistors;
using CircuitRenderer.Interfaces;
using CircuitRenderer.Items;
using Data.ACDCs.Interfaces;
using Instance;
using ModelSelection;
using Windowing.Components.Window;

public class QuickEditView : Grid
{
    private readonly Button _editButton;
    private readonly Button _modelButton;
    private readonly Label _unitDescriptionLabel;
    private readonly Label _unitLabel;
    private readonly Entry _valueEntry;
    private readonly Window? _window;
    private IWorksheetItem? _currentItem;
    private bool _isUpdating;
    private ModelSelectionWindow? _modelSelectionWindow;
    private SourceEditorWindow? _sourceEditorWindow;
    public Action? OnUpdatedValue { get; set; }
    public WindowContainer? ParentContainer { get; set; }

    public QuickEditView(Window? window)
    {
        _window = window;
        this.Margin(2)
            .Padding(2)
            .ColumnDefinitions(new ColumnDefinitionCollection
            {
                new(100),
                new(60),
                new(20),
                new(100),
                new(100),
                new()
            })
            .RowDefinitions(new RowDefinitionCollection { new(30) });

        _unitDescriptionLabel = new QuickEditLabel("Select item")
            .HorizontalTextAlignment(TextAlignment.End);
        Add(_unitDescriptionLabel);

        _valueEntry = new Entry("")
            .FontSize(15)
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill)
            .IsEnabled(true)
            .Margin(0)
            .Column(1)
            .OnTextChanged(ValueEntry_OnTextChanged);
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
    }

    public void Initialize()
    {
        _modelSelectionWindow = new ModelSelectionWindow(ParentContainer)
        {
            OnModelSelected = OnModelSelected,
            ZIndex = 10,
            OnClose = OnClose,
            IsVisible = false
        };
        _modelSelectionWindow.FadeTo(0);

        _sourceEditorWindow = new SourceEditorWindow(ParentContainer)
        {
            OnClose = OnClose,
            IsVisible = false,
            ZIndex = 10,
            OnSourceEdited = OnSourceEdited
        };
        _sourceEditorWindow.FadeTo(0);
    }

    public void UpdateEditor(IWorksheetItem item)
    {
        if (item is not WorksheetItem worksheetItem)
        {
            return;
        }

        _isUpdating = true;

        _currentItem = item;
        _unitDescriptionLabel.Text = "";
        _unitLabel.Text = "";
        _valueEntry.Text = "";
        _valueEntry.IsEnabled = true;
        _modelButton.Text = "Select model";

        string typeName = item.GetType().Name.Replace("Item", "");
        _window?.SetTitle($"{typeName} / {item.RefName}");

        switch (item)
        {
            case ResistorItem:
                UpdateFields(worksheetItem.Value.ParsePrefixesToDouble().ParseToPrefixedString(), "Resistance:", "Ω");
                break;

            case InductorItem:
                UpdateFields(worksheetItem.Value.ParsePrefixesToDouble().ParseToPrefixedString(), "Inductance:", "H");
                break;

            case CapacitorItem:
                UpdateFields(worksheetItem.Value.ParsePrefixesToDouble().ParseToPrefixedString(), "Capacity:", "F");
                break;

            case PnpTransistorItem:
            case NpnTransistorItem:
                UpdateFields(worksheetItem.Value, "Model:", "");
                _valueEntry.IsEnabled = false;
                break;

            case VoltageSourceItem:
                UpdateFields(worksheetItem.Value.ParsePrefixesToDouble().ParseToPrefixedString(), "Voltage:", "V");
                _modelButton.Text = "Edit source";
                break;

            default:
                UpdateFields("", "", "");
                break;
        }

        _isUpdating = false;
    }

    private void EditModelButton_Clicked(object? sender, EventArgs e)
    {
    }

    private bool OnClose(Window window)
    {
#pragma warning disable CS4014
        API.Call(async () =>
#pragma warning restore CS4014
        {
            await window.FadeTo(0);
            window.IsVisible = false;
        });

        return false;
    }

    private void OnModelSelected(IElectronicComponent component)
    {
        if (_currentItem is WorksheetItem item)
        {
            item.Model = component;
            item.Value = component.Value;
            OnUpdatedValue?.Invoke();
            UpdateEditor(item);
        }
    }

    private void OnSourceEdited(WorksheetItem item)
    {
    }

    private async void SelectModelButton_Clicked(object? sender, EventArgs e)
    {
        await API.Call(() =>
        {
            switch (_currentItem)
            {
                case null:
                    return Task.CompletedTask;

                case VoltageSourceItem voltageSource:
                    _sourceEditorWindow.IsVisible = true;
                    _sourceEditorWindow.FadeTo(1);
                    _sourceEditorWindow.SetSource(voltageSource);
                    API.TabBar?.BringToFront(_sourceEditorWindow);
                    return Task.CompletedTask;
            }

            _modelSelectionWindow.IsVisible = true;
            _modelSelectionWindow.FadeTo(1);
            _modelSelectionWindow.SetComponentType(_currentItem.GetType().Name);
            API.TabBar?.BringToFront(_modelSelectionWindow);

            return Task.CompletedTask;
        });
    }

    private void UpdateFields(string value, string description, string unit)
    {
        _valueEntry.Text = value;
        _unitDescriptionLabel.Text = description;
        _unitLabel.Text = unit;
    }

    private void ValueEntry_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (_isUpdating) return;
        if (string.IsNullOrEmpty(e.NewTextValue) || e.NewTextValue == e.OldTextValue) return;
        if (_currentItem is not WorksheetItem worksheetItem)
        {
            return;
        }

        switch (_currentItem)
        {
            case ResistorItem:
            case InductorItem:
            case CapacitorItem:
                if (worksheetItem.Model != null)
                {
                    worksheetItem.Value = e.NewTextValue.ParsePrefixesToDouble().ParseToPrefixedString();
                    worksheetItem.Model.Value = e.NewTextValue.ParsePrefixesToDouble().ToString();
                }
                break;
        }

        OnUpdatedValue?.Invoke();
    }
}
