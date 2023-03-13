// ReSharper disable StringLiteralTypo
namespace ACDCs.API.Core;

public static class UnitPrefixConverter
{
    private static readonly Dictionary<string, string> s_codeNames = new()
    {
        { "Q", "quetta" },
        { "R", "ronna" },
        { "Y", "yotta" },
        { "Z", "zetta" },
        { "E", "exa" },
        { "P", "peta" },
        { "T", "tera" },
        { "t", "tera" },
        { "G", "giga" },
        { "g", "giga" },
        { "M", "mega" },
        { "K", "kilo" },
        { "k", "kilo" },
        { "", "one" },
        { "m", "milli" },
        { "μ", "micro" },
        { "u", "micro" },
        { "n", "nano" },
        { "p", "pico" },
        { "f", "femto" },
        { "a", "atto" },
        { "z", "zepto" },
        { "y", "yocto" },
        { "r", "ronto" },
        { "q", "quecto" }
    };

    private static readonly Dictionary<string, int> s_exponents = new()
    {
        { "quetta", 30 },
        { "ronna", 27 },
        { "yotta", 24 },
        { "zetta", 21 },
        { "exa", 18 },
        { "peta", 15 },
        { "tera", 12 },
        { "giga", 9 },
        { "mega", 6 },
        { "kilo", 3 },
        { "one", 0 },
        { "milli", -3 },
        { "micro", -6 },
        { "nano", -9 },
        { "pico", -12 },
        { "femto", -15 },
        { "atto", -18 },
        { "zepto", -21 },
        { "yocto", -24 },
        { "ronto", -27 },
        { "quecto", -30 }
    };

    public static double ParsePrefixesToDouble(this string value)
    {
        value = value.Replace(",", ".");
        double numberValue = 0;
        string numberPart = "";
        char? lastPrefix = null;
        foreach (char chr in value)
        {
            string chrString = chr.ToString();
            if (int.TryParse(chrString, out int _) || chr == ',')
            {
                numberPart += chr;
            }
            else
            {
                if (string.IsNullOrEmpty(numberPart) ||
                    !s_codeNames.ContainsKey(chrString))
                {
                    continue;
                }

                lastPrefix = chr;
                int number = int.Parse(numberPart);
                numberPart = "";
                numberValue += number * Math.Pow(10, s_exponents[s_codeNames[chrString]]);
            }
        }

        switch (string.IsNullOrEmpty(numberPart))
        {
            case false when lastPrefix != null:
                {
                    double number = double.Parse(numberPart);
                    number /= 1000;
                    string? lastPrefixString = lastPrefix.ToString();
                    if (lastPrefixString != null && s_codeNames.ContainsKey(lastPrefixString))
                    {
                        numberValue += number * Math.Pow(10, s_exponents[s_codeNames[lastPrefixString]]);
                    }

                    break;
                }
            case false:
                numberValue = double.Parse(numberPart);
                break;
        }

        return numberValue;
    }

    public static string ParseToPrefixedString(this double value)
    {
        string prefixedValue = "";

        foreach (KeyValuePair<string, int> kvp in s_exponents)
        {
            double limit = Math.Pow(10, kvp.Value);
            if (!(value >= limit))
            {
                continue;
            }

            string symbol = s_codeNames.FirstOrDefault(s => s.Value == kvp.Key).Key;
            double calcedValue = value / limit;
            prefixedValue = $"{calcedValue}{symbol}";
            prefixedValue = prefixedValue.Replace(".", ",");

            break;
        }

        return prefixedValue;
    }
}
