namespace ACDCs.API.Interfaces;

using Sharp.UI;

[BindableProperties]
public interface IPropertyEditorViewProperties
{
    public Action<IPropertyEditorView> ModelEditorClicked { get; set; }
    public Action<IPropertyEditorView> ModelSelectionClicked { get; set; }
    public Action<object> OnValueChanged { get; set; }
    Type ParentType { get; set; }
    string PropertyName { get; set; }
    object Value { get; set; }
}
