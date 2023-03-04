namespace ACDCs.API.Core.Services;

using CircuitRenderer.Interfaces;
using CircuitRenderer.Items;
using CircuitRenderer.Sheet;
using Components.Circuit;
using Interfaces;

public class EditService : IEditService
{
    public async Task Delete(ICircuitView circuitView)
    {
        if (circuitView is not CircuitView circuit) return;

        Worksheet sheet = circuit.CurrentWorksheet;
        foreach (IWorksheetItem iitem in sheet.SelectedItems.ToList())
        {
            switch (iitem)
            {
                case TraceItem trace:
                    sheet.DeleteTrace(trace, circuit.SelectedTraceLine);
                    break;

                case WorksheetItem item:
                    sheet.DeleteItem(item);
                    break;
            }
        }

        sheet.StartRouter();
        await circuit.Paint();
    }

    public async Task DeselectAll(ICircuitView circuitView)
    {
        if (circuitView is not CircuitView circuit) return;

        circuit.CurrentWorksheet.SelectedItems.Clear();
        circuit.CurrentWorksheet.SelectedPin = null;
        await circuit.Paint();
    }

    public async Task Duplicate(ICircuitView circuitView)
    {
        if (circuitView is not CircuitView circuit) return;

        Worksheet sheet = circuit.CurrentWorksheet;

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
        await circuit.Paint();
    }

    public async Task Mirror(ICircuitView circuitView)
    {
        if (circuitView is not CircuitView circuit) return;

        Worksheet sheet = circuit.CurrentWorksheet;
        sheet.SelectedItems.ToList().ForEach(
            item => { sheet.MirrorItem((WorksheetItem)item); });
        sheet.StartRouter();
        await circuit.Paint();
    }

    public async Task Rotate(ICircuitView circuitView)
    {
        if (circuitView is not CircuitView circuit) return;

        Worksheet sheet = circuit.CurrentWorksheet;
        sheet.SelectedItems.ToList().ForEach(
            item => { sheet.RotateItem((WorksheetItem)item); });
        sheet.StartRouter();
        await circuit.Paint();
    }

    public async Task SelectAll(ICircuitView circuitView)
    {
        if (circuitView is not CircuitView circuit) return;

        circuit.CurrentWorksheet.SelectedItems.AddRange(
            circuit.CurrentWorksheet.Items);
        await circuit.Paint();
    }

    public async Task ShowProperties(ICircuitView circuitView)
    {
        if (circuitView is not CircuitView circuit) return;

        if (circuit.CurrentWorksheet.SelectedItems.Count == 0) return;

        circuit.ShowProperties(circuit.CurrentWorksheet.SelectedItems.First());
        await circuit.Paint();
    }

    public async Task SwitchMultiselect(object? state, ICircuitView circuitView)
    {
        if (circuitView is not CircuitView circuit) return;
        circuit.UseMultiselect((bool)(state ?? false));
        await Task.Delay(0);
    }
}
