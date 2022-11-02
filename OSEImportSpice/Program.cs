using SpiceSharp.Entities;
using SpiceSharpParser;
using SpiceSharpParser.ModelReaders.Netlist.Spice;
using Newtonsoft.Json;
using System.Diagnostics;
using SpiceSharp.Documentation;
using SpiceSharp;
using OSEData.OSE.Components;
using System.Text.Json.Serialization;
using JsonConverter = Newtonsoft.Json.JsonConverter;
using Newtonsoft.Json.Linq;
using System.Reflection;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using SpiceSharp.Components;

namespace OSEImportSpice
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                var types = typeof(DiodeModel).Assembly.GetTypes()
                                       .Where(t => t.Namespace != null && t.Namespace.StartsWith("SpiceSharp.Components"));

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

                        var output = JsonConvert.SerializeObject(Activator.CreateInstance(type, argsi), jsonsettings);
                        string dirName = "models";

                        if (!Directory.Exists("./json/" + dirName))
                        {
                            Directory.CreateDirectory("./json/" + dirName);
                        }
                        AddOutput("./json/" + dirName + "/" + type.Name + ".json", output);
                        Console.WriteLine(type.Name);
                    }
                    catch (Exception e)
                    {
                        //  Console.WriteLine(e);
                    }
                }

                return;
            }

            SpiceSharpParser.SpiceNetlistParser parser = new SpiceNetlistParser();

            var result = parser.ParseNetlist(File.ReadAllText(args[0]));
            if (result.ValidationResult.HasError)
            {
                result.ValidationResult.Errors.ToList().ForEach(
                    e => Console.WriteLine(e.Message + "-" + e.LineInfo.LineNumber));
            }
            else
            {
                Console.WriteLine(result);
                SpiceNetlistReaderSettings settings = new SpiceNetlistReaderSettings();

                var jsonsettings = new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Include,
                    Formatting = Formatting.Indented,
                    TypeNameHandling = TypeNameHandling.None,
                    ContractResolver = new ConverterContractResolver(),
                };
                var reader = new SpiceSharpReader(settings);
                var spiceSharpModel = reader.Read(result.InputModel);
                foreach (IEntity? cModel in spiceSharpModel.Circuit)
                {
                    try
                    {
                        var output = JsonConvert.SerializeObject(cModel, jsonsettings);
                        var test = JsonConvert.DeserializeObject<Bjt>(output, jsonsettings);
                        string dirName = cModel.GetType().Name;
                        if (!Directory.Exists("./json/" + dirName))
                        {
                            Directory.CreateDirectory("./json/" + dirName);
                        }
                        AddOutput("./json/" + dirName + "/" + cModel.Name + ".json", output);
                        Console.WriteLine(cModel.Name);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
        }

        private static void AddOutput(string v, string output)
        {
            File.WriteAllText(v, output);
        }
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

        public override bool CanRead
        {
            get { return false; }
        }
    }
}