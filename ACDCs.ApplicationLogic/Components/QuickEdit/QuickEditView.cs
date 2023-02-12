namespace ACDCs.ApplicationLogic.Components.QuickEdit;

using ACDCs.CircuitRenderer.Interfaces;
using Sharp.UI;

public class QuickEditView : Grid
{
    private readonly Label _labelName;
    private readonly Label _unitLabel;
    private readonly Entry _valueEntry;

    public QuickEditView()
    {
        this.Margin(2)
        .ColumnDefinitions(new ColumnDefinitionCollection
        {
            new ColumnDefinition(120),
            new ColumnDefinition(60),
            new ColumnDefinition(10),
            new ColumnDefinition()
        })
        .RowDefinitions(new RowDefinitionCollection
        {
            new RowDefinition(40) ,
            new RowDefinition(20)
        });

        _labelName = new QuickEditLabel("Select item");
        Add(_labelName);

        _valueEntry = new Entry("---")
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill)
            .IsEnabled(true)
            .Margin(0)
            .Column(1);
        Add(_valueEntry);

        _unitLabel = new QuickEditLabel("---")
            .Column(2);
        Add(_unitLabel);
    }

    public void UpdateEditor(IWorksheetItem item)
    {
        string typeName = item.GetType().Name.Replace("Item", "");
        _labelName.Text = $"{typeName}/{item.RefName}";
    }
}

public class QuickEditLabel : Label
{
    public QuickEditLabel(string text)
    {
        this.Text(text);
    }
}
