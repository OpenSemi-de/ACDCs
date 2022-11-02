using OSECircuitRender;
using System;

namespace HelloWold
{
    public static class Program
    {
        public static void Main()
        {
            OSECircuitRender.Log.Method = Console.WriteLine;
            Workbook wb = new();
            Worksheet ws = wb.AddNewSheet();
            ResistorItem ri = new("10k", 10, 10);
            ws.Items.AddItem(ri);
            PinItem pi = new(PinDrawableType.GND, 10, 60);

            ws.Items.AddItem(pi);

            //Console.WriteLine(JsonConvert.SerializeObject(wb, Formatting.Indented));
            //	Console.WriteLine(JsonConvert.SerializeObject(wb.Sheets.First().GetDrawableComponents(), Formatting.Indented));
            ws.CalculateScene();
            //Console.WriteLine(JsonConvert.SerializeObject(wb.Sheets[0].SceneManager.GetSceneForBackend(), Formatting.Indented));
        }
    }
}