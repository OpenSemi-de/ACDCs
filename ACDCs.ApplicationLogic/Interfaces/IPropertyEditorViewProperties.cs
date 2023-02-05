﻿namespace ACDCs.ApplicationLogic.Interfaces;

using Components.Properties;
using Sharp.UI;

[BindableProperties]
public interface IPropertyEditorViewProperties
{
    public Action<PropertyEditorView> ModelEditorClicked { get; set; }
    public Action<PropertyEditorView> ModelSelectionClicked { get; set; }
    public Action<object> OnValueChanged { get; set; }
    Type ParentType { get; set; }
    string PropertyName { get; set; }
    object Value { get; set; }
}
