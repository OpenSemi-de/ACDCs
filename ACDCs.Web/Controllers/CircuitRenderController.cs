using ACDCs.CircuitRenderer;
using ACDCs.CircuitRenderer.Drawables;
using ACDCs.CircuitRenderer.Items;
using ACDCs.CircuitRenderer.Scene;
using ACDCs.CircuitRenderer.Sheet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Skia;

namespace ACDCs.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CircuitRenderController : Controller
{
    private readonly IWebHostEnvironment _environment;

    public CircuitRenderController(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    [HttpGet]
    public FileContentResult Image()
    {
        byte[]? imageBytes = null;
        SkiaBitmapExportContext? debugContext = new(1000, 1000, 10);

        Log.Method = Console.WriteLine;
        Workbook.DebugContext = debugContext;
        Workbook wb = new();

        Worksheet ws = wb.AddNewSheet();
        string? wwwPath = _environment.WebRootPath;
        string contentPath = _environment.ContentRootPath;

        Workbook.BasePath = wwwPath;

        ResistorItem res1 = new("1k", 1, 1);

        ResistorItem res2 = new("1k", 1, 4) { Rotation = 90f };

        ResistorItem res3 = new("1k", 1, 7) { Rotation = 135f };

        ResistorItem res4 = new("1k", 1, 10);

        ws.Items.AddItem(res1);
        ws.Items.AddItem(res2);
        ws.Items.AddItem(res3);
        ws.Items.AddItem(res4);

        TerminalItem gnd1 = new(TerminalDrawableType.Gnd, 6, 1);

        TerminalItem gnd4 = new(TerminalDrawableType.Null, 6, 10);

        ws.Items.AddItem(gnd1);
        ws.Items.AddItem(gnd4);

        InductorItem ind = new("10m", 10, 1);
        ws.Items.AddItem(ind);

        DiodeItem dio = new("0.7", 10, 4);
        ws.Items.AddItem(dio);

        TransistorItem pnp = new(TransistorDrawableType.Pnp, 10, 7);
        ws.Items.AddItem(pnp);

        TransistorItem npn = new(TransistorDrawableType.Npn, 10, 11);
        ws.Items.AddItem(npn);
        TransistorItem npnr = new(TransistorDrawableType.Npn, 10, 15) { Rotation = -90f };

        ws.Items.AddItem(npnr);

        NetItem net2 = new();
        net2.Pins.Add(res1.Pins[0].ComponentGuid);
        net2.Pins.Add(res2.Pins[1].ComponentGuid);
        net2.Pins.Add(pnp.Pins[0].ComponentGuid);
        net2.Pins.Add(npn.Pins[2].ComponentGuid);
        ws.Nets.AddItem(net2);

        CapacitorItem caps = new("10u", CapacitorDrawableType.Standard, 14, 1);
        ws.Items.AddItem(caps);
        CapacitorItem caps2 = new("10u", CapacitorDrawableType.Polarized, 14, 4);
        ws.Items.AddItem(caps2);

        if (System.IO.File.Exists(wwwPath + "/input.json"))
        {
            ws = wb.LoadSheet(wwwPath + "/input.json");
        }

        if (ws != null)
        {
            ws.CalculateScene();
            DrawableScene? scene = (DrawableScene?)ws.SceneManager?.GetSceneForBackend();

            SkiaBitmapExportContext context = new(1000, 1000, 1);
            scene?.Draw(context.Canvas, RectF.Zero);

            using (MemoryStream ms = new())
            {
                context.WriteToStream(ms);
                ms.Position = 0;
                imageBytes = ms.ToArray();
            }

            context.Dispose();

            wb.SaveSheet(ws, wwwPath + "/samplesheet.json");
        }

        using (MemoryStream ms = new())
        {
            debugContext.WriteToStream(ms);
            ms.Position = 0;
            byte[] mapImageBytes = ms.ToArray();
            System.IO.File.WriteAllBytes(wwwPath + "/map.bmp", mapImageBytes);
        }

        return File(imageBytes ?? Array.Empty<byte>(), "application/octet-stream", "img.bmp");
    }
}
