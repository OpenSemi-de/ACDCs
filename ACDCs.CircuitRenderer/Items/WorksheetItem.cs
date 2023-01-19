#nullable enable

using System;
using System.Linq;
using ACDCs.CircuitRenderer.Drawables;
using ACDCs.CircuitRenderer.Interfaces;
using ACDCs.Data.ACDCs.Components;
using Newtonsoft.Json;

namespace ACDCs.CircuitRenderer.Items;

public class WorksheetItem : IWorksheetItem
{
    private string? _value;

    public virtual string DefaultValue { get; set; } = string.Empty;

    [JsonIgnore]
    public IDrawableComponent DrawableComponent { get; set; }

    public int Height
    {
        get => Convert.ToInt32(DrawableComponent.Size.Y);
        set => DrawableComponent.Size.Y = value;
    }

    public virtual bool IsInsertable { get; set; }

    public bool IsMirrored
    {
        get => DrawableComponent.IsMirrored;
        set => DrawableComponent.IsMirrored = value;
    }

    public Guid ItemGuid { get; set; } = Guid.NewGuid();
    public IElectronicComponent? Model { get; set; }
    public string Name { get; set; } = string.Empty;

    [JsonIgnore] public DrawablePinList Pins { get; set; } = new();

    public string RefName { get; set; } = string.Empty;

    public float Rotation
    {
        get => DrawableComponent.Rotation;
        set => DrawableComponent.Rotation = value;
    }

    public string TypeName => GetType().Name.Replace("Item", "");

    public string? Value
    {
        get => _value ?? DefaultValue;
        set
        {
            if (value != null)
            {
                DrawableComponent.Value = value;
                if (Model != null)
                {
                    Model.Value = ParseUnits(value);
                }
            }

            _value = value ?? DefaultValue;
        }
    }

    public int Width
    {
        get => Convert.ToInt32(DrawableComponent.Size.X);
        set => DrawableComponent.Size.X = value;
    }

    public int X
    {
        get => Convert.ToInt32(DrawableComponent.Position.X);
        set => DrawableComponent.Position.X = value;
    }

    public int Y
    {
        get => Convert.ToInt32(DrawableComponent.Position.Y);
        set => DrawableComponent.Position.Y = value;
    }

    public WorksheetItem()
    {
        DrawableComponent = new DrawableComponent(typeof(DrawableComponent), this);
    }

    public static string ParseUnits(string stringValue)
    {
        double partialValue = 0;
        double result = 0;
        double multiplier = 0;

        stringValue = stringValue.Trim();

        foreach (string ch in stringValue.Select(c => Convert.ToString(c)))
        {
            switch (ch)
            {
                case "k":
                case "K":
                    multiplier = 1000;
                    partialValue = PartialValue(partialValue, multiplier, ref result);
                    break;

                case "M":
                    multiplier = 1000000;
                    partialValue = PartialValue(partialValue, multiplier, ref result);
                    break;

                case "G":
                case "g":
                    multiplier = 1000000000;
                    partialValue = PartialValue(partialValue, multiplier, ref result);
                    break;

                case "m":
                    multiplier = 1d / 1000;
                    partialValue = PartialValue(partialValue, multiplier, ref result);
                    break;

                case "n":
                    multiplier = 1d / 1000000;
                    partialValue = PartialValue(partialValue, multiplier, ref result);
                    break;

                case "u":
                    multiplier = 1d / 1000000000;
                    partialValue = PartialValue(partialValue, multiplier, ref result);
                    break;

                case "p":
                    multiplier = 1d / 1000000000000;
                    partialValue = PartialValue(partialValue, multiplier, ref result);
                    break;

                default:
                    {
                        if (int.TryParse(ch, out int intValue))
                        {
                            if (partialValue == 0)
                            {
                                partialValue = intValue;
                            }
                            else
                            {
                                partialValue *= 10;
                                partialValue += intValue;
                            }
                        }

                        break;
                    }
            }
        }

        if (partialValue != 0 && result != 0)
        {
            partialValue *= multiplier / 10;
            result += partialValue;
        }

        if (result == 0) { result = partialValue; }

        return Convert.ToString(result);
    }

    private static double PartialValue(double partialValue, double multiplier, ref double result)
    {
        partialValue *= multiplier;
        result += partialValue;
        partialValue = 0;
        return partialValue;
    }
}
