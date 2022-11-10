using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OSECircuitRender;
using Microsoft.Maui.Graphics.Skia;
using Microsoft.Maui.Graphics;
using System.Net;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using OSECircuitRender.Drawables;
using OSECircuitRender.Interfaces;
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
            SkiaBitmapExportContext debugcontext = new(100, 100, 10);

            OSECircuitRender.Log.Method = Console.WriteLine;
            OSECircuitRender.Workbook.DebugContext = debugcontext;
            Workbook wb = new();

            Worksheet ws = wb.AddNewSheet();
            string wwwPath = environment.WebRootPath;
            string contentPath = environment.ContentRootPath;

            Workbook.BasePath = wwwPath;

            var res1 = new ResistorItem("1k", 1, 1);

            var res2 = new ResistorItem("1k", 1, 4)
            {
                Rotation = 90f
            };

            var res3 = new ResistorItem("1k", 1, 7)
            {
                Rotation = 135f
            };

            var res4 = new ResistorItem("1k", 1, 10);

            ws.Items.AddItem(res1);
            ws.Items.AddItem(res2);
            ws.Items.AddItem(res3);
            ws.Items.AddItem(res4);

            //var gnd1 = new PinItem(PinDrawableType.Gnd, 6, 1);
            //   var gnd2 = new PinItem(PinDrawableType.Pin, 6, 4);
            //  var gnd3 = new PinItem(PinDrawableType.None, 6, 7);
            var gnd4 = new TerminalItem(TerminalDrawableType.Null, 6, 10);

            //       ws.Items.AddItem(gnd1);
            //  ws.Items.AddItem(gnd2);
            //  ws.Items.AddItem(gnd3);
            ws.Items.AddItem(gnd4);

            var ind = new InductorItem("10m", 10, 1);
            ws.Items.AddItem(ind);

            var dio = new DiodeItem("0.7", 10, 4);
            ws.Items.AddItem(dio);

            var pnp = new TransistorItem(TransistorDrawableType.PNP, 10, 7);
            ws.Items.AddItem(pnp);

            var npn = new TransistorItem(TransistorDrawableType.NPN, 10, 11);
            ws.Items.AddItem(npn);
            var npnr = new TransistorItem(TransistorDrawableType.NPN, 10, 15);

            npnr.Rotation = -90f;
            ws.Items.AddItem(npnr);

            var net2 = new NetItem();
            net2.Pins.Add(res1.Pins[0]);
            net2.Pins.Add(res2.Pins[1]);
            net2.Pins.Add(pnp.Pins[0]);
            net2.Pins.Add(npn.Pins[2]);
            ws.Nets.AddItem(net2);

            var caps = new CapacitorItem("10u", CapacitorDrawableType.Standard, 14, 1);
            ws.Items.AddItem(caps);
            var caps2 = new CapacitorItem("10u", CapacitorDrawableType.Polarized, 14, 4);
            ws.Items.AddItem(caps2);

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
                context.Dispose();

                wb.SaveSheet(ws, wwwPath + "/samplesheet.json");
            }
            return File(imageBytes, "application/octet-stream", "img.bmp");
        }
    }
}