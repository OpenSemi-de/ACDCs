namespace ACDCs.ApplicationLogic.Services;

using CircuitRenderer.Interfaces;
using CircuitRenderer.Items;
using CircuitRenderer.Sheet;
using Components.Circuit;
using Interfaces;

public class EditService : IEditService
{
    public async Task Delete(CircuitView circuitView)
    {
        Worksheet sheet = circuitView.CurrentWorksheet;
        foreach (IWorksheetItem iitem in sheet.SelectedItems.ToList())
        {
            switch (iitem)
            {
                case TraceItem trace:
                    sheet.DeleteTrace(trace, circuitView.SelectedTraceLine);
                    break;

                case WorksheetItem item:
                    sheet.DeleteItem(item);
                    break;
            }
        }

        sheet.StartRouter();
        await circuitView.Paint();
    }

    public async Task DeselectAll(CircuitView circuitView)
    {
        circuitView.CurrentWorksheet.SelectedItems.Clear();
        circuitView.CurrentWorksheet.SelectedPin = null;
        await circuitView.Paint();
    }

    public async Task Duplicate(CircuitView circuitView)
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
            if (item == null)
            {
                return;
            }

            item.X += 2;
            item.Y += 2;
            sheet.SelectItem(item);
        });
        sheet.StartRouter();
        await circuitView.Paint();
    }

    public async Task Mirror(CircuitView circuitView)
    {
        Worksheet sheet = circuitView.CurrentWorksheet;
        sheet.SelectedItems.ToList().ForEach(
            item => { sheet.MirrorItem((WorksheetItem)item); });
        sheet.StartRouter();
        await circuitView.Paint();
    }

    public async Task Rotate(CircuitView circuitView)
    {
        Worksheet sheet = circuitView.CurrentWorksheet;
        sheet.SelectedItems.ToList().ForEach(
            item => { sheet.RotateItem((WorksheetItem)item); });
        sheet.StartRouter();
        await circuitView.Paint();
    }

    public async Task SelectAll(CircuitView circuitView)
    {
        circuitView.CurrentWorksheet.SelectedItems.AddRange(
            circuitView.CurrentWorksheet.Items);
        await circuitView.Paint();
    }

    public async Task ShowProperties(CircuitView circuitView)
    {
        if (circuitView.CurrentWorksheet.SelectedItems.Count == 0) return;

        circuitView.ShowProperties(circuitView.CurrentWorksheet.SelectedItems.First());
        await circuitView.Paint();
    }

    public async Task SwitchMultiselect(object? state, CircuitView circuitView)
    {
        circuitView.UseMultiselect((bool)(state ?? false));
        await Task.Delay(0);
    }
}
