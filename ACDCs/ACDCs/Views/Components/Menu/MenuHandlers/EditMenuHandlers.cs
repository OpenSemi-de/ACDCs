using System.Linq;
using Microsoft.Maui.Controls;
using OSECircuitRender.Items;
using OSECircuitRender.Sheet;

namespace ACDCs.Views.Components.Menu.MenuHandlers;

public class EditMenuHandlers : ContentView
{
    public EditMenuHandlers()
    {
        MenuHandler.Add("delete", Delete);
    }

    private void Delete()
    {

        var sheet = App.Com<Worksheet>("CircuitView", "CurrentWorksheet");
        if (sheet != null)
            sheet.SelectedItems.ToList().ForEach(
                item => { sheet.DeleteItem((WorksheetItem)item); });
        App.Com<CircuitView.CircuitView>("CircuitView", "Instance").Paint();
    }
}