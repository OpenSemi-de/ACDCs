namespace ACDCs.ApplicationLogic.Components.QuickEdit;

using ACDCs.CircuitRenderer.Interfaces;
using Sharp.UI;

public class QuickEditView : Grid
{
    private readonly Label _labelName;

    public QuickEditView()
    {
        this.ColumnDefinitions(new ColumnDefinitionCollection
        {
            new ColumnDefinition(120),
            new ColumnDefinition(140),
            new ColumnDefinition()
        });

        this.RowDefinitions(new RowDefinitionCollection { new RowDefinition(60) });
        _labelName = new QuickEditLabel("Select item");
        Add(_labelName);
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
