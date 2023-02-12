namespace ACDCs.ApplicationLogic.Components.QuickEdit;

using Sharp.UI;

public class QuickEditView : Grid
{
    public QuickEditView()
    {
        this.ColumnDefinitions(new ColumnDefinitionCollection
        {
            new ColumnDefinition(60),
            new ColumnDefinition(140),
            new ColumnDefinition()
        });

        this.RowDefinitions(new RowDefinitionCollection { new RowDefinition(60) });
    }
}
