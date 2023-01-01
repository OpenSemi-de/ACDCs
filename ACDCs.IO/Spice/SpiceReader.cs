using ACDCs.Data.ACDCs.Components;
using AutoMapper;
using SpiceSharp.Components;
using SpiceSharp.Entities;
using SpiceSharpParser;
using SpiceSharpParser.Common.Validation;
using SpiceSharpParser.ModelReaders.Netlist.Spice;
using Capacitor = ACDCs.Data.ACDCs.Components.Capacitor;
using Diode = ACDCs.Data.ACDCs.Components.Diode;
using Resistor = ACDCs.Data.ACDCs.Components.Resistor;

namespace ACDCs.IO.Spice;

public class SpiceReader
{
    private Mapper _mapper;
    private IEnumerable<Profile> _profiles;
    public List<ValidationEntry>? Errors { get; set; }

    public bool HasErrors { get; set; }

    public SpiceReader()
    {
        _profiles = new List<Profile>()
        {
            new MapperConfigurationExpression()
        };

        IConfigurationProvider configuration =
            new MapperConfiguration(configure =>
            {
                configure.CreateMap<SpiceSharp.Components.Bipolars.ModelParameters, Bjt>();
                configure.CreateMap<SpiceSharp.Components.Diodes.ModelParameters, Diode>();
                configure.CreateMap<SpiceSharp.Components.Capacitors.ModelParameters, Capacitor>();
                configure.CreateMap<SpiceSharp.Components.Resistors.ModelParameters, Resistor>();
            });

        _mapper = new Mapper(configuration);
    }

    public List<IElectronicComponent> ReadComponents(string netlistData)
    {
        List<IElectronicComponent> components = new();
        List<IEntity> models = ReadModels(netlistData);
        foreach (IEntity model in models)
        {
            switch (model)
            {
                case BipolarJunctionTransistorModel bjtModel:
                    Bjt bjt = _mapper.Map<Bjt>(bjtModel.Parameters);
                    bjt.Name = bjtModel.Name;
                    components.Add(bjt);
                    break;

                case DiodeModel diodeModel:
                    Diode diode = _mapper.Map<Diode>(diodeModel.Parameters);
                    diode.Name = diodeModel.Name;
                    components.Add(diode);
                    break;

                case CapacitorModel capacitorModel:
                    Capacitor capacitor = _mapper.Map<Capacitor>(capacitorModel.Parameters);
                    capacitor.Name = capacitorModel.Name;
                    components.Add(capacitor);
                    break;

                case ResistorModel resistorModel:
                    Resistor resistor = _mapper.Map<Resistor>(resistorModel.Parameters);
                    resistor.Name = resistorModel.Name;
                    components.Add(resistor);
                    break;
            }
        }
        return components;
    }

    public List<IEntity> ReadModels(string netlistData)
    {
        SpiceNetlistParserSettings sett = new()
        {
            Parsing = { IsEndRequired = false, IsNewlineRequired = true }
        };
        SpiceNetlistParser parser = new(sett);

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
