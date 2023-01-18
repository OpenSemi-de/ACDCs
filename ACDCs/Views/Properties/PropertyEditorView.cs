using ACDCs.Data.ACDCs.Components;
using ACDCs.Interfaces;

namespace ACDCs.Views.Properties;

using Sharp.UI;

[SharpObject]
public partial class PropertyEditorView : ContentView, IPropertyEditorViewProperties
{
    private int _fontSize;

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

    public void OnModelSelected(IElectronicComponent model)
    {
        OnValueChanged?.Invoke(model);
        Value = model;
    }

    private void Entry_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (e.OldTextValue != e.NewTextValue && e.OldTextValue != "")
        {
            OnValueChanged?.Invoke(e.NewTextValue);
        }
    }

    private void Knob_OnValueChanged(object obj)
    {
        OnValueChanged?.Invoke(obj);
        Value = obj;
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
            if (e.PropertyName != null && e.PropertyName.StartsWith("Value"))
            {
                object? value = GetValue(ValueProperty);
                if (value != null)
                {
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

                        foreach (var enumValue in value.GetType().GetEnumValues())
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
                        string text = component != null ? component.Name : "";
                        Button modelButton = new Button()
                            .HorizontalOptions(LayoutOptions.Fill)
                            .VerticalOptions(LayoutOptions.Fill)
                            .Margin(0)
                            .FontSize(_fontSize)
                            .Text(text)
                            .OnClicked(ModelButton_Clicked);

                        ValueType = c.GetType().Name + (c.Type != "" ? ":" + c.Type : "");
                        Content = modelButton;
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
                }
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
