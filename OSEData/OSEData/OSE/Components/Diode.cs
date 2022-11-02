using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSEData.OSE.Components
{
    public class Diode : ElectricalComponent, IElectronicComponent
    {
        public IComponentRuntimeParameters ParametersRuntime => new DiodeRuntimeParameters();
        public IComponentParameters ParametersModel => new DiodeParameters();
    }

    public class DiodeRuntimeParameters : IComponentRuntimeParameters
    {
        public bool Off { get; set; }
        public float TemperatureCelsius { get; set; }
        public float InitCond { get; set; }
        public float ParallelMultiplier { get; set; }
        public float SeriesMultiplier { get; set; }
        public float Area { get; set; }
        public float Temperature { get; set; }
    }

    public class DiodeParameters : IComponentParameters
    {
        public float NominalTemperatureCelsius { get; set; }
        public float TransitTime { get; set; }
        public float GradingCoefficient { get; set; }
        public float DepletionCapCoefficient { get; set; }
        public float SaturationCurrent { get; set; }
        public float EmissionCoefficient { get; set; }
        public float JunctionPotential { get; set; }
        public float NominalTemperature { get; set; }
        public float FlickerNoiseExponent { get; set; }
        public float SaturationCurrentExp { get; set; }
        public float Resistance { get; set; }
        public float ActivationEnergy { get; set; }
        public float JunctionCap { get; set; }
        public float BreakdownCurrent { get; set; }
        public float FlickerNoiseCoefficient { get; set; }
        public float BreakdownVoltage { get; set; }
    }
}