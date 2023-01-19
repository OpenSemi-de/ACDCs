namespace ACDCs.Data.ACDCs.Components.Resistor;

public static class ResistorCalculator
{
    private static readonly ResistorTolerance s_tolerance = new();
    private static readonly ResistorValues s_values = new();
    private static List<Resistor>? s_resistors;

    public static List<Resistor> GetAllValues()
    {
        if (s_resistors == null)
        {
            List<Resistor> resistors = new();
            foreach (string resistorSeries in Enum.GetNames(typeof(ResistorSeries)))
            {
                ResistorSeries series = Enum.Parse<ResistorSeries>(resistorSeries);
                foreach (double baseValue in s_values[series])
                {
                    foreach (double tolerance in s_tolerance[series])
                    {
                        for (double multiplicator = 1d; multiplicator < 1000000000; multiplicator *= 10)
                        {
                            double value = baseValue * multiplicator;
                            Resistor resistor = new()
                            {
                                Name = $"{GetStringValue(value)}/{series}/{tolerance}%",
                                Series = series,
                                Multiplicator = multiplicator,
                                Tolerance = tolerance,
                                Value = Convert.ToString(value),
                            };
                            resistors.Add(resistor);
                        }
                    }
                }
            }

            s_resistors = resistors;
        }

        return s_resistors;
    }

    public static string GetStringValue(double value)
    {
        if (value >= 1000000000)
        {
            return value / 1000000000 + "G";
        }

        if (value >= 1000000)
        {
            return value / 1000000 + "M";
        }

        if (value >= 1000)
        {
            return value / 1000 + "K";
        }

        return Convert.ToString(value);
    }
}
