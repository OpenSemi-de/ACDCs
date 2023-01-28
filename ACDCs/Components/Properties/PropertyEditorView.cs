using ACDCs.Components.Window;
using ACDCs.Data.ACDCs.Interfaces;
using ACDCs.Interfaces;
using ACDCs.Services;
using ACDCs.Views.ModelEditor;
using ACDCs.Views.ModelSelection;
using Sharp.UI;
using Button = Sharp.UI.Button;
using ColumnDefinition = Sharp.UI.ColumnDefinition;
using ContentView = Sharp.UI.ContentView;
using Entry = Sharp.UI.Entry;
using Grid = Sharp.UI.Grid;
using Label = Sharp.UI.Label;
using Picker = Sharp.UI.Picker;
using RowDefinition = Sharp.UI.RowDefinition;
using Switch = Sharp.UI.Switch;

namespace ACDCs.Components.Properties;

[SharpObject]
public partial class PropertyEditorView : ContentView, IPropertyEditorViewProperties
{
    private ModelEditorWindowView? _modelEditorWindow;
    private ModelSelectionWindowView? _modelSelectionWindow;
    private WindowView? _parentWindow;
    public bool ShowDescription { get; set; }
    public string ValueType { get; set; } = string.Empty;

    public PropertyEditorView()
    {
        Initialize();
    }

    public PropertyEditorView(WindowView? parentWindow)
    {
        Initialize(parentWindow);
    }

    public void OnModelEdited(IElectronicComponent model)
    {
        OnValueChanged?.Invoke(model);
        Value = model;
    }

    public void OnModelSelected(IElectronicComponent model)
    {
        OnValueChanged?.Invoke(model);
        Value = model;
    }

    private void EditModel_Clicked(object? sender, EventArgs e)
    {
        if (_modelEditorWindow == null)
        {
            _modelEditorWindow = new ModelEditorWindowView(_parentWindow?.MainContainer ?? API.MainContainer)
            {
                OnModelEdited = OnModelEdited,
                ZIndex = 10
            };

            _modelEditorWindow.GetProperties(Value as IElectronicComponent);

            _modelEditorWindow.OnClose = () =>
            {
                _modelEditorWindow = null;
                return false;
            };
        }
        else
        {
            API.TabBar?.BringToFront(_modelEditorWindow);
        }
    }

    private void Entry_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (e.OldTextValue != e.NewTextValue && e.OldTextValue != "")
        {
            OnValueChanged?.Invoke(e.NewTextValue);
        }
    }

    private void Initialize(WindowView? parentWindow = null)
    {
        _parentWindow = parentWindow;
        PropertyChanged += PropertyEditor_PropertyChanged;
        Unloaded += OnUnloaded;
    }

    private void ModelButton_Clicked(object? sender, EventArgs e)
    {
        if (_modelSelectionWindow == null)
        {
            _modelSelectionWindow = new ModelSelectionWindowView(_parentWindow?.MainContainer ?? API.MainContainer)
            {
                OnModelSelected = OnModelSelected,
                ZIndex = 10
            };

            _modelSelectionWindow.SetComponentType(Value.GetType().Name);

            _modelSelectionWindow.OnClose = () =>
            {
                _modelSelectionWindow = null;
                return false;
            };
        }
        else
        {
            API.TabBar?.BringToFront(_modelSelectionWindow);
        }
    }

    private void OnUnloaded(object? sender, EventArgs e)
    {
        _modelSelectionWindow = null;
        _modelEditorWindow = null;
        _parentWindow = null;
        OnValueChanged = null;
        PropertyChanged -= PropertyEditor_PropertyChanged;
        Unloaded -= OnUnloaded;
    }

    private void Picker_OnSelectedIndexChange(object? sender, EventArgs e)
    {
        if (sender is Picker picker)
        {
            OnValueChanged?.Invoke(picker.SelectedItem);
        }
    }

    private async void PropertyEditor_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        await API.Call(() =>
        {
            if (e.PropertyName == null || !e.PropertyName.StartsWith("Value"))
            {
                return Task.CompletedTask;
            }

            object? value = GetValue(ValueProperty);
            if (value == null)
            {
                return Task.CompletedTask;
            }

            ValueType = value.GetType().Name;

            Grid grid = new Grid()
                .HorizontalOptions(LayoutOptions.Fill)
                .VerticalOptions(LayoutOptions.Fill)
                .Margin(0)
                .Padding(0);
            grid.ColumnDefinitions.Add(new ColumnDefinition(100));
            grid.RowDefinitions.Add(new RowDefinition(34));

            Content = grid;

            if (PropertyName == "Value")
            {
                grid.Add(new Entry()
                    .HorizontalOptions(LayoutOptions.Fill)
                    .VerticalOptions(LayoutOptions.Fill)

                    .OnTextChanged(Value_OnTextChanged)
                    .Text(Convert.ToString(value)));
            }
            else if (value is bool boolValue)
            {
                grid.Add(new Switch()
                    .HorizontalOptions(LayoutOptions.Start)
                    .VerticalOptions(LayoutOptions.Start)
                    .IsToggled(boolValue)
                    .OnToggled(Toggle_OnToggle));
            }
            else if (value.GetType().IsEnum)
            {
                Picker? picker = new Picker()
                    .HorizontalOptions(LayoutOptions.Fill)
                    .VerticalOptions(LayoutOptions.Fill)
                    .Margin(0);

                foreach (object? enumValue in value.GetType().GetEnumValues())
                {
                    picker.Items.Add(Convert.ToString(enumValue));
                }

                picker.SelectedItem = Convert.ToString(value);
                picker.OnSelectedIndexChanged(Picker_OnSelectedIndexChange);
                grid.Add(picker);
            }
            // else if (value is int intValue)
            // {
            //     TurnKnob knob = new TurnKnob()
            //         .InputValue(value)
            //         .Margin(0)
            //         .WidthRequest(Width)
            //         .HeightRequest(Height)
            //         .OnKnobValueChanged(Knob_OnValueChanged);
            //
            //     Content = knob;
            // }
            else if (value is IElectronicComponent c)
            {
                string text = c.Name != "" ? c.Name : "Select model";
                Button modelButton = new Button()
                    .HorizontalOptions(LayoutOptions.Fill)
                    .VerticalOptions(LayoutOptions.Fill)
                    .Margin(0)
                    .Padding(0)
                    .Text(text)
                    .OnClicked(ModelButton_Clicked);

                Button editButton = new Button("Edit model")
                    .HorizontalOptions(LayoutOptions.Fill)
                    .VerticalOptions(LayoutOptions.Fill)
                    .Margin(0)
                    .Padding(0)
                    .OnClicked(EditModel_Clicked);

                Grid.SetRow(editButton, 1);

                ValueType = c.GetType().Name + (c.Type != "" ? ":" + c.Type : "");

                grid.RowDefinitions.Add(new RowDefinition(34));

                grid.Add(modelButton);
                grid.Add(editButton);

                Content = grid;
            }
            else
            {
                grid.Add(new Entry().
                    HorizontalOptions(LayoutOptions.Fill).VerticalOptions(LayoutOptions.Fill).OnTextChanged(Entry_OnTextChanged).Text(Convert.ToString(value)));
            }

            if (!ShowDescription)
            {
                return Task.CompletedTask;
            }

            grid.ColumnDefinitions.Add(new ColumnDefinition().Width(GridLength.Star));
            Label descriptionLabel = new Label()
                .HorizontalOptions(LayoutOptions.Fill)
                .VerticalOptions(LayoutOptions.Fill)
                .Text(DescriptionService.GetComponentDescription(ParentType, PropertyName));
            Grid.SetColumn(descriptionLabel, 2);
            grid.Add(descriptionLabel);

            return Task.CompletedTask;
        });
    }

    private void Toggle_OnToggle(object? sender, ToggledEventArgs e)
    {
        OnValueChanged?.Invoke(e.Value);
    }

    private void Value_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        OnValueChanged?.Invoke(e.NewTextValue);
    }
}
