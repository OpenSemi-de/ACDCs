using OSECircuitRender.Items;
using OSECircuitRender.Sheet;
using System.Collections.Generic;
using System.Linq;

namespace ACDCs.Views.Components.Menu.MenuHandlers;

public class EditMenuHandlers : MenuHandlerView
{
    public EditMenuHandlers()
    {
        MenuHandler.Add("delete", Delete);
        MenuHandler.Add("duplicate", Duplicate);
        MenuHandler.Add("selectall", SelectAll);
        MenuHandler.Add("deselectall", DeselectAll);
    }

    private async void Delete()
    {
        Worksheet sheet = CircuitView.CurrentWorksheet;
        sheet.SelectedItems.ToList().ForEach(
            item => { sheet.DeleteItem((WorksheetItem)item); });
        await CircuitView.Paint();
    }

    private async void DeselectAll()
    {
        CircuitView.CurrentWorksheet.SelectedItems.Clear();
        await CircuitView.Paint();
    }

    private async void Duplicate()
    {
        Worksheet sheet = CircuitView.CurrentWorksheet;

        List<WorksheetItem?> newItems = new();
        sheet.SelectedItems.ToList().ForEach(
            item =>
            {
                newItems.Add(sheet.DuplicateItem((WorksheetItem)item));
            }
        );

        newItems.ForEach(item =>
        {
            if (item != null) sheet.Items.Add(item);
        });

        sheet.SelectedItems.ToList().ForEach(item => sheet.DeselectItem((WorksheetItem)item));
        newItems.ForEach(item =>
        {
            if (item != null)
            {
                item.X += 2;
                item.Y += 2;
                sheet.SelectItem(item);
            }
        });

        await CircuitView.Paint();
    }

    private async void SelectAll()
    {
        CircuitView.CurrentWorksheet.SelectedItems.AddRange(
            CircuitView.CurrentWorksheet.Items);
        await CircuitView.Paint();
    }
}