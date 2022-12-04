using System.Linq;
using OSECircuitRender.Items;
using OSECircuitRender.Sheet;

namespace ACDCs.Views.Components.Menu.MenuHandlers;

public class EditMenuHandlers : MenuHandlerView
{
    public EditMenuHandlers()
    {
        MenuHandler.Add("delete", Delete);
    }

    private void Delete()
    {

        Worksheet sheet = CircuitView.CurrentWorksheet;
        sheet.SelectedItems.ToList().ForEach(
            item => { sheet.DeleteItem((WorksheetItem)item); });
        CircuitView.Paint();
    }
}