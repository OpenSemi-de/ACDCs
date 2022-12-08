using System;
using System.IO;
using ACDCs.CircuitRenderer.Sheet;
using Microsoft.Maui.Graphics.Skia;
using Newtonsoft.Json;

namespace ACDCs.CircuitRenderer;

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

    public static string? BasePath { get; set; }
    public static SkiaBitmapExportContext? DebugContext { get; set; }

    public static string BaseFontName { get; set; } = "";

    public Worksheet AddNewSheet()
    {
        Worksheet ws = new();
        Log.L("Adding sheet");
        Sheets.AddSheet(ws);
        return ws;
    }

    public Worksheet LoadSheet(string fileName)
    {
        string? json = File.ReadAllText(fileName);
        var ws = JsonConvert.DeserializeObject<Worksheet>(json, _jsonSerializerSettings);

        if (ws != null)
        {
            Sheets.AddSheet(ws);
            return ws;
        }

        return AddNewSheet();
    }

    public void SaveSheet(Worksheet ws, string fileName)
    {
        string? json = JsonConvert.SerializeObject(ws, _jsonSerializerSettings);
        File.WriteAllText(fileName, json);
    }

    public void SetBaseFont(string fontName)
    {
        BaseFontName = fontName;
    }
}

public static class Log
{
    public static Action<string>? Method;

    public static void L(string text)
    {
        Method?.Invoke(text);
    }
}
