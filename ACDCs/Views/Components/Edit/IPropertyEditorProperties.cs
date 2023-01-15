namespace ACDCs.Views.Components.Edit;

using Sharp.UI;

[BindableProperties]
public interface IPropertyEditorProperties
{
    string PropertyName { get; set; }
    object Value { get; set; }
}
