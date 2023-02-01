using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace ACDCs.ApplicationLogic.Components.Properties;

public class PropertyItem : INotifyPropertyChanged, INotifyCollectionChanged
{
    public ObservableCollection<PropertyItem> Children { get; } = new();
    public bool IsExpanded { get; set; }
    public bool IsLeaf { get; set; }
    public string Name { get; } = string.Empty;
    public int Order { get; set; }
    public Type? ParentType { get; set; }
    public object? Value { get; set; }

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
