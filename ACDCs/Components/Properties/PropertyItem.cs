using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace ACDCs.Components.Properties;

public class PropertyItem : INotifyPropertyChanged, INotifyCollectionChanged
{
    public ObservableCollection<PropertyItem> Children { get; set; } = new();
    public bool IsExpanded { get; set; }
    public bool IsLeaf { get; set; }
    public string Name { get; set; } = string.Empty;
    public Type ParentType { get; set; }
    public object? Value { get; set; } = null;

    public PropertyItem(string? name)
    {
        if (name != null)
        {
            Name = name;
        }
    }

    public event NotifyCollectionChangedEventHandler? CollectionChanged
    {
        add => Children.CollectionChanged += value;
        remove => Children.CollectionChanged -= value;
    }

    public event PropertyChangedEventHandler? PropertyChanged
    {
        add => ((INotifyPropertyChanged)Children).PropertyChanged += value;
        remove => ((INotifyPropertyChanged)Children).PropertyChanged -= value;
    }
}
