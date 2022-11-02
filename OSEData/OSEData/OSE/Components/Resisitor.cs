using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSEData.OSE.Components
{
    public class Resisitor : ElectricalComponent, IElectronicComponent
    {
        public IComponentRuntimeParameters ParametersRuntime => throw new NotImplementedException();
        public IComponentParameters ParametersModel => new ResisitorParameters();
    }

    public class ResisitorParameters : IComponentParameters
    {
        public float NominalTemperatureCelsius { get; set; }
        public float TemperatureCoefficient1 { get; set; }
        public float TemperatureCoefficient2 { get; set; }
        public float ExponentialCoefficient { get; set; }
        public float SheetResistance { get; set; }
        public float Narrow { get; set; }
        public float DefaultWidth { get; set; }
        public float NominalTemperature { get; set; }
    }
}