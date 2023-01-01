namespace ACDCs.Data.ACDCs.Components;

public class Diode : DiodeParameters, IElectronicComponent
{
    public string Model { get; set; }
    public string Name { get; set; }
    public IComponentRuntimeParameters ParametersRuntime => new DiodeRuntimeParameters();
    public string Type { get; set; }
}

public class DiodeParameters : IComponentParameters
{
    public double ActivationEnergy { get; set; }
    public double BreakdownCurrent { get; set; }
    public double BreakdownVoltage { get; set; }
    public double DepletionCapCoefficient { get; set; }
    public double EmissionCoefficient { get; set; }
    public double FlickerNoiseCoefficient { get; set; }
    public double FlickerNoiseExponent { get; set; }
    public double GradingCoefficient { get; set; }
    public double JunctionCap { get; set; }
    public double JunctionPotential { get; set; }
    public double NominalTemperature { get; set; }
    public double NominalTemperatureCelsius { get; set; }
    public double Resistance { get; set; }
    public double SaturationCurrent { get; set; }
    public double SaturationCurrentExp { get; set; }
    public double TransitTime { get; set; }
}

public class DiodeRuntimeParameters : IComponentRuntimeParameters
{
    public double Area { get; set; }
    public double InitCond { get; set; }
    public bool Off { get; set; }
    public double ParallelMultiplier { get; set; }
    public double SeriesMultiplier { get; set; }
    public double Temperature { get; set; }
    public double TemperatureCelsius { get; set; }
}
