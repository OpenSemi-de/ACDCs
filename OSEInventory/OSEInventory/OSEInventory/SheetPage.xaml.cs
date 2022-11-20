// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Graphics.Skia;
using OSECircuitRender;
using OSECircuitRender.Scene;

namespace OSEInventory;

public partial class SheetPage : ContentPage
{
    public SheetPage()
    {
        InitializeComponent();
        SetupSheet();
    }

    private void SetupSheet()
    {
        if (App.CurrentWorkbook == null)
        {
            App.CurrentWorkbook = new Workbook();
        }

        App.CurrentSheet = App.CurrentWorkbook.AddNewSheet();

        App.CurrentSheet.CalculateScene();
        DrawableScene scene = (DrawableScene)
            App.CurrentSheet.SceneManager.GetSceneForBackend();

        sheetGraphicsView.Drawable = scene;

    }
}
