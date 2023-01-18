using ACDCs.CircuitRenderer.Items;
using ACDCs.CircuitRenderer.Sheet;
using ACDCs.Views.Circuit;

namespace ACDCs.Services;

public static class EditService
{
    public static async Task Delete(CircuitView circuitView)
    {
        Worksheet sheet = circuitView.CurrentWorksheet;
        sheet.SelectedItems.ToList().ForEach(
            item => { sheet.DeleteItem((WorksheetItem)item); });
        sheet.StartRouter();
        await circuitView.Paint();
    }

    public static async Task DeselectAll(CircuitView circuitView)
    {
        circuitView.CurrentWorksheet.SelectedItems.Clear();
        circuitView.CurrentWorksheet.SelectedPin = null;
        await circuitView.Paint();
    }

    public static async Task Duplicate(CircuitView circuitView)
    {
        Worksheet sheet = circuitView.CurrentWorksheet;

        List<WorksheetItem?> newItems = new();
        sheet.SelectedItems.ToList().ForEach(
            item => { newItems.Add(sheet.DuplicateItem((WorksheetItem)item)); }
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
        sheet.StartRouter();
        await circuitView.Paint();
    }

    public static async Task Mirror(CircuitView circuitView)
    {
        Worksheet sheet = circuitView.CurrentWorksheet;
        sheet.SelectedItems.ToList().ForEach(
            item => { sheet.MirrorItem((WorksheetItem)item); });
        sheet.StartRouter();
        await circuitView.Paint();
    }

    public static async Task Rotate(CircuitView circuitView)
    {
        Worksheet sheet = circuitView.CurrentWorksheet;
        sheet.SelectedItems.ToList().ForEach(
            item => { sheet.RotateItem((WorksheetItem)item); });
        sheet.StartRouter();
        await circuitView.Paint();
    }

    public static async Task SelectAll(CircuitView circuitView)
    {
        circuitView.CurrentWorksheet.SelectedItems.AddRange(
            circuitView.CurrentWorksheet.Items);
        await circuitView.Paint();
    }

    public static async Task SwitchMultiselect(object? state, CircuitView circuitView)
    {
        circuitView.UseMultiselect((bool)(state ?? false));
        await Task.Delay(0);
    }
}
