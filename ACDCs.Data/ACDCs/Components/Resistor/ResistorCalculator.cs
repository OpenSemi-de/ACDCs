namespace ACDCs.Data.ACDCs.Components.Resistor;

public static class ResistorCalculator
{
    private static readonly ResistorTolerance s_tolerance = new();
    private static readonly ResistorValues s_values = new();
    private static List<Resistor>? s_resistors;

    public static List<Resistor> GetAllValues()
    {
        if (s_resistors != null)
        {
            return s_resistors;
        }

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
                            Value = Convert.ToString(value)
                        };
                        resistors.Add(resistor);
                    }
                }
            }
        }

        s_resistors = resistors;

        return s_resistors;
    }

    public static string GetStringValue(double value)
    {
        return value switch
        {
            >= 1000000000 => value / 1000000000 + "G",
            >= 1000000 => value / 1000000 + "M",
            >= 1000 => value / 1000 + "K",
            _ => Convert.ToString(value)
        };
    }
}
