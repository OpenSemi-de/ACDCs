using System;
using OSECircuitRender.Drawables;
using OSECircuitRender.Items;
using OSECircuitRender.Sheet;

namespace OSECircuitRender
{
    public static class Program
    {
        public static void Main()
        {
            Log.Method = Console.WriteLine;
            Workbook wb = new();
            Worksheet ws = wb.AddNewSheet();
            ResistorItem ri = new("10k", 10, 10);
            ws.Items.AddItem(ri);
            PinItem pi = new(PinDrawableType.Gnd, 10, 60);

            ws.Items.AddItem(pi);

            ws.CalculateScene();
        }
    }
}