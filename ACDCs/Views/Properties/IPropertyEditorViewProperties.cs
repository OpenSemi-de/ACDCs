namespace ACDCs.Views.Properties;

using Sharp.UI;

[BindableProperties]
public interface IPropertyEditorViewProperties
{
    string PropertyName { get; set; }
    object Value { get; set; }
}
