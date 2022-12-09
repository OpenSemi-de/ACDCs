// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Linq;
using ACDCs.CircuitRenderer.Instructions;
using ACDCs.CircuitRenderer.Items;

namespace ACDCs.CircuitRenderer.Drawables;

public class TextDrawable : DrawableComponent
{
    public TextDrawable(WorksheetItem parent, string value, float textSize, float x, float y) : base(
        typeof(TextDrawable), parent)
    {
        DrawInstructions.Add(new TextInstruction(value, 0f, textSize, 0.5f, 0.5f));
        SetSize(2, 1);
        SetPosition(x, y);
    }

    public bool IsRealFontSize
    {
        get => ((TextInstruction)DrawInstructions.First()).IsRealFontSize;
        set => ((TextInstruction)DrawInstructions.First()).IsRealFontSize = value;
    }
}
