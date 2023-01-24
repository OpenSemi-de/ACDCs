using Sharp.UI;

namespace ACDCs.Interfaces;

[BindableProperties]
public interface IPropertyEditorViewProperties
{
    Type ParentType { get; set; }
    string PropertyName { get; set; }
    object Value { get; set; }
}
