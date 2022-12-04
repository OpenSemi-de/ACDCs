using System.Collections.Generic;
using System.Linq;
using OSECircuitRender.Items;
using OSECircuitRender.Sheet;

namespace ACDCs.Views.Components.Menu.MenuHandlers;

public class EditMenuHandlers : MenuHandlerView
{
    public EditMenuHandlers()
    {
        MenuHandler.Add("delete", Delete);
        MenuHandler.Add("duplicate", Duplicate);
    }

    private void Delete()
    {

        Worksheet sheet = CircuitView.CurrentWorksheet;
        sheet.SelectedItems.ToList().ForEach(
            item => { sheet.DeleteItem((WorksheetItem)item); });
        CircuitView.Paint();
    }

    private void Duplicate()
    {
        Worksheet sheet = CircuitView.CurrentWorksheet;

        List<WorksheetItem?> newItems = new();
        sheet.SelectedItems.ToList().ForEach(
            item => { newItems.Add(sheet.DuplicateItem((WorksheetItem)item)); }
        );

        newItems.ForEach(item => sheet.Items.Add(item));

        sheet.SelectedItems.ForEach(item => sheet.DeselectItem((WorksheetItem)item));
        newItems.ForEach(item =>
        {
            if (item != null)
            {
                item.X += 2;
                item.Y += 2;
                sheet.SelectItem(item);
            }
        });

        CircuitView.Paint();
    }

}