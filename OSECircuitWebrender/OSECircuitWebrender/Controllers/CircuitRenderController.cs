using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OSECircuitRender;
using Microsoft.Maui.Graphics.Skia;
using Microsoft.Maui.Graphics;
using System.Net;
using System.Net.Http.Headers;
using OSECircuitRender.Drawables;
using OSECircuitRender.Items;
using OSECircuitRender.Scene;
using OSECircuitRender.Sheet;
using static System.Net.Mime.MediaTypeNames;

namespace OSECircuitWebrender.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CircuitRenderController : Controller
    {
        private IWebHostEnvironment environment;

        public CircuitRenderController(IWebHostEnvironment _environment)
        {
            environment = _environment;
        }

        [HttpGet]
        public FileContentResult Image()
        {
            byte[]? imageBytes = null;

            OSECircuitRender.Log.Method = Console.WriteLine;
            Workbook wb = new();
            Worksheet ws = wb.AddNewSheet();
            string wwwPath = environment.WebRootPath;
            string contentPath = environment.ContentRootPath;

            var res1 = new ResistorItem("1k", 1, 1);
            var res2 = new ResistorItem("1k", 1, 4);
            var res3 = new ResistorItem("1k", 1, 7);
            var res4 = new ResistorItem("1k", 1, 10);

            ws.Items.AddItem(res1);
            ws.Items.AddItem(res2);
            ws.Items.AddItem(res3);
            ws.Items.AddItem(res4);

            var gnd1 = new PinItem(PinDrawableType.Gnd, 3, 1);
            var gnd2 = new PinItem(PinDrawableType.Gnd, 3, 4);
            var gnd3 = new PinItem(PinDrawableType.Gnd, 3, 7);
            var gnd4 = new PinItem(PinDrawableType.Gnd, 3, 10);

            ws.Items.AddItem(gnd1);
            ws.Items.AddItem(gnd2);
            ws.Items.AddItem(gnd3);
            ws.Items.AddItem(gnd4);

            var ind = new InductorItem("10m", 5, 1);
            ws.Items.AddItem(ind);

            if (System.IO.File.Exists(wwwPath + "/input.json"))
            {
                ws = wb.LoadSheet(wwwPath + "/input.json");
            }
            if (ws != null)
            {
                ws.CalculateScene();
                DrawableScene scene = (DrawableScene)ws.SceneManager.GetSceneForBackend();

                SkiaBitmapExportContext context = new(1000, 1000, 1);
                scene.Draw(context.Canvas, RectF.Zero);

                using (MemoryStream ms = new())
                {
                    context.WriteToStream(ms);
                    ms.Position = 0;
                    imageBytes = ms.ToArray();
                }

                wb.SaveSheet(ws, wwwPath + "/samplesheet.json");
            }
            return File(imageBytes, "application/octet-stream", "img.bmp");
        }
    }
}