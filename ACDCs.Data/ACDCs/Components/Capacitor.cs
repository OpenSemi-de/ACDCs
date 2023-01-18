﻿namespace ACDCs.Data.ACDCs.Components;

public class Capacitor : CapacitorParameters, IElectronicComponent
{
    public string Model { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public IComponentRuntimeParameters ParametersRuntime => new CapacitorRuntimeParameters();
    public string Type { get; set; } = string.Empty;

    public string Value
    {
        get => Convert.ToString(((CapacitorRuntimeParameters)ParametersRuntime).Capacitance);
        set
        {
            ((CapacitorRuntimeParameters)ParametersRuntime).Capacitance = Convert.ToDouble(value);
        }
    }
}

public class CapacitorParameters : IComponentParameters
{
    public double DefaultWidth { get; set; }
    public double JunctionCap { get; set; }
    public double JunctionCapSidewall { get; set; }
    public double Narrow { get; set; }
    public double NominalTemperature { get; set; }
    public double NominalTemperatureCelsius { get; set; }
    public double TemperatureCoefficient1 { get; set; }
    public double TemperatureCoefficient2 { get; set; }
}

public class CapacitorRuntimeParameters : IComponentRuntimeParameters
{
    public double Capacitance { get; set; }
    public double InitialCondition { get; set; }
    public double Length { get; set; }
    public double ParallelMultiplier { get; set; }
    public double Temperature { get; set; }
    public double TemperatureCelsius { get; set; }
    public double Width { get; set; }
}
