using Microsoft.Maui.Graphics.Skia;
using Newtonsoft.Json;
using OSECircuitRender.Sheet;
using System;
using System.IO;

namespace OSECircuitRender;

public sealed class Workbook
{
    public static readonly float BaseGridSize = 2.54f;
    public static readonly float Zoom = 10f;
    public WorksheetsList Sheets = new();
    private readonly JsonSerializerSettings _jsonSerializerSettings;

    public Workbook()
    {
        _jsonSerializerSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            Formatting = Formatting.Indented
        };
    }

    public static string BasePath { get; set; }
    public static SkiaBitmapExportContext DebugContext { get; set; }

    public Worksheet AddNewSheet()
    {
        Worksheet ws = new();
        Log.L("Adding sheet");
        Sheets.AddSheet(ws);
        return ws;
    }

    public Worksheet LoadSheet(string fileName)
    {
        var json = File.ReadAllText(fileName);
        var ws = JsonConvert.DeserializeObject<Worksheet>(json, _jsonSerializerSettings);

        Sheets.AddSheet(ws);
        return ws;
    }

    public void SaveSheet(Worksheet ws, string fileName)
    {
        var json = JsonConvert.SerializeObject(ws, _jsonSerializerSettings);
        File.WriteAllText(fileName, json);
    }
}

public static class Log
{
    public static Action<string> Method;

    public static void L(string text)
    {
        Method?.Invoke(text);
    }
}