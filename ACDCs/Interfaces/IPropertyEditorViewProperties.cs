using Sharp.UI;

namespace ACDCs.Interfaces;

[BindableProperties]
public interface IPropertyEditorViewProperties
{
    string PropertyName { get; set; }
    object Value { get; set; }
}
