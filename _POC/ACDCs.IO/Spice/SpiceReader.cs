using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using ACDCs.Data.ACDCs.Components.BJT;
using ACDCs.Data.ACDCs.Interfaces;
using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using SpiceSharp.Components;
using SpiceSharp.Entities;
using SpiceSharpParser;
using SpiceSharpParser.Common.Validation;
using SpiceSharpParser.ModelReaders.Netlist.Spice;
using Capacitor = ACDCs.Data.ACDCs.Components.Capacitor.Capacitor;
using Diode = ACDCs.Data.ACDCs.Components.Diode.Diode;
using Resistor = ACDCs.Data.ACDCs.Components.Resistor.Resistor;

namespace ACDCs.IO.Spice;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class SpiceReader
{
    private readonly Mapper _mapper;
    public List<ValidationEntry>? Errors { get; set; }

    public bool HasErrors { get; set; }

    public SpiceReader()
    {
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

    public void CreateJson()
    {
        var types = typeof(BipolarJunctionTransistor).Assembly.GetTypes()
            .Where(t => t.Namespace != null);

        foreach (var type in types)
        {
            try
            {
                Console.WriteLine(type.Name);

                var jsonsettings = new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Include,
                    Formatting = Formatting.Indented,
                    TypeNameHandling = TypeNameHandling.None,
                    ContractResolver = new ConverterContractResolver(),
                };

                string[] argsi = new string[1] { type.Name };

                object? instance = Activator.CreateInstance(type, argsi);
                if (instance == null)
                    instance = Activator.CreateInstance(type);
                var output = JsonConvert.SerializeObject(instance, jsonsettings);
                string dirName = "models";
                string basePath = FileSystem.AppDataDirectory;

                if (!Directory.Exists(basePath + "/json/" + dirName))
                {
                    Directory.CreateDirectory(basePath + "/json/" + dirName);
                }
                AddOutput(basePath + "/json/" + dirName + "/" + type.Name + ".json", output);
                Console.WriteLine(type.Name);
            }
            catch (Exception e)
            {
                //  Console.WriteLine(e);
            }
        }
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
        var spiceSharpModel = reader.Read(result.FinalModel);
        return spiceSharpModel.Circuit.ToList();
    }

    private static void AddOutput(string v, string output)
    {
        File.WriteAllText(v, output);
    }

    public class ConverterContractResolver : DefaultContractResolver
    {
        public new static readonly ConverterContractResolver Instance = new ConverterContractResolver();

        protected override JsonContract CreateContract(Type objectType)
        {
            JsonContract contract = base.CreateContract(objectType);

            // this will only be called once and then cached
            if (objectType.Name.StartsWith("Given"))
            {
                contract.Converter = new GivenValueConverter();
            }

            return contract;
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var prop = base.CreateProperty(member, memberSerialization);
            if (member.Name.StartsWith("ParameterSet"))
            {
                prop.ShouldSerialize = i => false;
                prop.Ignored = true;
            }

            return prop;
        }
    }

    public class GivenValueConverter : JsonConverter
    {
        public override bool CanRead
        {
            get { return false; }
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            var valProp = value.GetType().GetProperty("Value");
            var givProp = value.GetType().GetProperty("Given");
            bool given = (bool)givProp.GetValue(value, null);
            JToken t;
            var ovalue = valProp.GetValue(value, null);
            if (given)
                t = JToken.FromObject(ovalue);
            else
                t = JToken.FromObject(Activator.CreateInstance(ovalue.GetType()));
            t.WriteTo(writer);
        }
    }
}
