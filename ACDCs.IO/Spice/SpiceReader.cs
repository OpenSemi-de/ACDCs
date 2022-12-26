using SpiceSharp.Entities;
using SpiceSharpParser;
using SpiceSharpParser.Common.Validation;
using SpiceSharpParser.ModelReaders.Netlist.Spice;

namespace ACDCs.IO.Spice;

public class SpiceReader
{
    public List<ValidationEntry>? Errors { get; set; }

    public bool HasErrors { get; set; }

    public List<IEntity> ReadModels(string netlistData)
    {
        SpiceNetlistParser parser = new();

        var result = parser.ParseNetlist(netlistData);
        if (result.ValidationResult.HasError)
        {
            HasErrors = true;
            Errors = result.ValidationResult.Errors.ToList();
            return new List<IEntity>();
        }

        SpiceNetlistReaderSettings settings = new();
        var reader = new SpiceSharpReader(settings);
        var spiceSharpModel = reader.Read(result.InputModel);
        return spiceSharpModel.Circuit.ToList();
    }
}
