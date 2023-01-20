using ACDCs.Data.ACDCs.Interfaces;
using ACDCs.Interfaces;

namespace ACDCs.Views.Properties;

using Sharp.UI;

[SharpObject]
public partial class PropertyEditorView : ContentView, IPropertyEditorViewProperties
{
    private int _fontSize;

    public Action<PropertyEditorView>? OnModelEditorClicked { get; set; }
    public Action<PropertyEditorView>? OnModelSelectionClicked { get; set; }
    public Action<object>? OnValueChanged { get; set; }

    public string ValueType { get; set; } = string.Empty;

    public PropertyEditorView()
    {
        PropertyChanged += PropertyEditor_PropertyChanged;
    }

    public PropertyEditorView Fontsize(int fontSize)
    {
        _fontSize = fontSize;
        return this;
    }

    public void OnModelEdited(IElectronicComponent obj)
    {
    }

    public void OnModelSelected(IElectronicComponent model)
    {
        OnValueChanged?.Invoke(model);
        Value = model;
    }

    private void EditModel_Clicked(object? sender, EventArgs e)
    {
        OnModelEditorClicked?.Invoke(this);
    }

    private void Entry_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (e.OldTextValue != e.NewTextValue && e.OldTextValue != "")
        {
            OnValueChanged?.Invoke(e.NewTextValue);
        }
    }

    private void ModelButton_Clicked(object? sender, EventArgs e)
    {
        OnModelSelectionClicked?.Invoke(this);
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

            if (PropertyName == "Value")
            {
                Content = new Entry()
                    .HorizontalOptions(LayoutOptions.Fill)
                    .VerticalOptions(LayoutOptions.Fill)
                    .FontSize(_fontSize)
                    .OnTextChanged(Value_OnTextChanged)
                    .Text(Convert.ToString(value));
            }
            else if (value is bool boolValue)
            {
                Content = new Switch()
                    .HorizontalOptions(LayoutOptions.Start)
                    .VerticalOptions(LayoutOptions.Start)
                    .IsToggled(boolValue)
                    .OnToggled(Toggle_OnToggle);
            }
            else if (value.GetType().IsEnum)
            {
                Picker? picker = new Picker()
                    .HorizontalOptions(LayoutOptions.Fill)
                    .VerticalOptions(LayoutOptions.Fill)
                    .Margin(0)
                    .FontSize(_fontSize);

                foreach (object? enumValue in value.GetType().GetEnumValues())
                {
                    picker.Items.Add(Convert.ToString(enumValue));
                }

                picker.SelectedItem = Convert.ToString(value);
                picker.OnSelectedIndexChanged(Picker_OnSelectedIndexChange);
                Content = picker;
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
                IElectronicComponent? component = (IElectronicComponent?)c;
                string text = "Select model";
                Button modelButton = new Button()
                    .HorizontalOptions(LayoutOptions.Fill)
                    .VerticalOptions(LayoutOptions.Fill)
                    .Margin(0)
                    .FontSize(_fontSize)
                    .Text(text)
                    .OnClicked(ModelButton_Clicked);

                Button editButton = new Button("Edit model")
                    .HorizontalOptions(LayoutOptions.Fill)
                    .VerticalOptions(LayoutOptions.Fill)
                    .Margin(0)
                    .FontSize(_fontSize)
                    .OnClicked(EditModel_Clicked);

                Grid.SetRow(editButton, 1);

                ValueType = c.GetType().Name + (c.Type != "" ? ":" + c.Type : "");

                Grid grid = new Grid()
                    .HorizontalOptions(LayoutOptions.Fill)
                    .VerticalOptions(LayoutOptions.Fill)
                    .Margin(0)
                    .Padding(0);
                grid.RowDefinitions.Add(new RowDefinition(34));
                grid.RowDefinitions.Add(new RowDefinition(34));

                grid.Add(modelButton);
                grid.Add(editButton);

                Content = grid;
            }
            else
            {
                Content = new Entry()
                    .HorizontalOptions(LayoutOptions.Fill)
                    .VerticalOptions(LayoutOptions.Fill)
                    .FontSize(_fontSize)
                    .OnTextChanged(Entry_OnTextChanged)
                    .Text(Convert.ToString(value));
            }

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
