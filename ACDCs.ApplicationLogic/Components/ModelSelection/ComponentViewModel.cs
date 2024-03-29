﻿namespace ACDCs.API.Core.Components.ModelSelection;

using Data.ACDCs.Interfaces;

[BindableProperties]
public class ComponentViewModel
{
    public Color ItemBackground { get; set; } = Colors.Transparent;
    public IElectronicComponent? Model { get; set; }
    public string? Name { get; set; } = string.Empty;
    public int Row { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}
