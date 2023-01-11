namespace ACDCs.Data.ACDCs.Components.Resistor
{
    public class ResistorRuntimeParameters : IComponentRuntimeParameters
    {
        public double Length { get; set; }

        public double ParallelMultiplier { get; set; }
        public double Resistance { get; set; }
        public double SeriesMultiplier { get; set; }

        public double Temperature { get; set; }
        public double TemperatureCelsius { get; set; }
        public double Width { get; set; }
    }
}
