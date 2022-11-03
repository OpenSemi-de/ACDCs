﻿using Microsoft.Maui.Storage;
using Newtonsoft.Json;
using System;
using System.IO;

namespace OSECircuitRender
{
    public sealed class Workbook
    {
        public Worksheets Sheets = new();

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
            Worksheet ws = JsonConvert.DeserializeObject<Worksheet>(json);
            
            Sheets.AddSheet(ws);
            return ws;            
        }

        public void SaveSheet(Worksheet ws, string fileName)
        {
            var json = JsonConvert.SerializeObject(ws, Formatting.Indented);
            File.WriteAllText(fileName, json);
        }
    }

    public static class Log
    {
        public static void L(string text)
        {
            Method?.Invoke(text);
        }

        public static Action<string> Method;
    }
}