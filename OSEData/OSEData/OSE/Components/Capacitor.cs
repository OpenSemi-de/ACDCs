using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSEData.OSE.Components
{
    public class Capacitor : ElectricalComponent, IElectronicComponent
    {
        public string Name { get; set; }
        public IComponentRuntimeParameters ParametersRuntime => new CapacitorRuntimeParameters();
        public IComponentParameters ParametersModel => new CapacitorParameters();
    }

    public class CapacitorRuntimeParameters : IComponentRuntimeParameters
    {
        public float TemperatureCelsius { get; set; }
        public float InitialCondition { get; set; }
        public float Length { get; set; }
        public float Capacitance { get; set; }
        public float Temperature { get; set; }
        public float Width { get; set; }
        public float ParallelMultiplier { get; set; }
    }

    public class CapacitorParameters : IComponentParameters
    {
        public float Narrow { get; set; }
        public float TemperatureCoefficient1 { get; set; }
        public float TemperatureCoefficient2 { get; set; }
        public float NominalTemperatureCelsius { get; set; }
        public float DefaultWidth { get; set; }
        public float JunctionCapSidewall { get; set; }
        public float JunctionCap { get; set; }
        public float NominalTemperature { get; set; }
    }
}