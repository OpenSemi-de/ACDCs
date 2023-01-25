using System.Reflection;
using ACDCs.Data.ACDCs.Components.BJT;
using ACDCs.Data.ACDCs.Components.Inductor;
using ACDCs.Data.ACDCs.Components.Resistor;
using ACDCs.Data.ACDCs.Interfaces;
using SpiceSharp.Attributes;

namespace ACDCs.Services;

public static class DescriptionService
{
    public static Dictionary<Type, Type> DescriptionTypes = new()
    {
        {typeof(Resistor), typeof(SpiceSharp.Components.Resistors.Parameters)},
        {typeof(Bjt), typeof(SpiceSharp.Components.Bipolars.Parameters)},
        {typeof(Inductor), typeof(SpiceSharp.Components.Inductors.Parameters)}
    };

    public static string GetComponentDescription(Type parentType, string propertyName)
    {
        if (parentType.GetInterfaces().All(i => i != typeof(IElectronicComponent)))
        {
            return "";
        }

        if (!DescriptionTypes.ContainsKey(parentType))
        {
            return "";
        }

        Type targetNamespaceType = DescriptionTypes[parentType];
        List<Type> targetTypes = targetNamespaceType.Assembly.GetTypes()
            .Where(type => type.Namespace == targetNamespaceType.Namespace).ToList();

        foreach (Type targetType in targetTypes)
        {
            PropertyInfo? property = targetType.GetProperty(propertyName);
            if (property == null)
            {
                continue;
            }

            var parameterInfo =
                property.CustomAttributes.FirstOrDefault(attr => attr.AttributeType == typeof(ParameterInfoAttribute));

            if (parameterInfo == null)
            {
                continue;
            }

            return Convert.ToString(parameterInfo.ConstructorArguments.First().Value) ?? string.Empty;
        }

        return "";
    }
}
