﻿using System.IO;
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

    public static string BaseFontName { get; set; } = string.Empty;

    public static string? BasePath { get; set; }

    public static SkiaBitmapExportContext? DebugContext { get; set; }

    public Workbook()
    {
        _jsonSerializerSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            Formatting = Formatting.Indented
        };
    }

    public Worksheet AddNewSheet()
    {
        Worksheet ws = new();
        Log.L("Adding sheet");
        Sheets.AddSheet(ws);
        return ws;
    }

    public Worksheet LoadSheet(string fileName)
    {
        string json = File.ReadAllText(fileName);
        Worksheet? ws = JsonConvert.DeserializeObject<Worksheet>(json, _jsonSerializerSettings);

        if (ws == null)
        {
            return AddNewSheet();
        }

        Sheets.AddSheet(ws);
        ws.StartRouter();
        return ws;
    }

    public void SaveSheet(Worksheet ws, string fileName)
    {
        string json = JsonConvert.SerializeObject(ws, _jsonSerializerSettings);
        File.WriteAllText(fileName, json);
    }

    public void SetBaseFont(string fontName)
    {
        BaseFontName = fontName;
    }
}
