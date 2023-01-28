using ACDCs.Data.ACDCs.Components.BJT;
using ACDCs.Data.ACDCs.Components.Inductor;
using ACDCs.Data.ACDCs.Components.Resistor;
using ACDCs.Data.ACDCs.Interfaces;
using SpiceSharp.Attributes;

namespace ACDCs.Services;

public static class DescriptionService
{
    private static readonly Dictionary<Type, Type> s_descriptionTypes = new()
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

        if (!s_descriptionTypes.ContainsKey(parentType))
        {
            return "";
        }

        Type targetNamespaceType = s_descriptionTypes[parentType];
        List<Type> targetTypes = targetNamespaceType.Assembly.GetTypes()
            .Where(type => type.Namespace == targetNamespaceType.Namespace).ToList();

        foreach (var parameterInfo in targetTypes.Select(targetType => targetType.GetProperty(propertyName))
                     .Where(property => property != null)
                     .Select(property =>
                         property.CustomAttributes.FirstOrDefault(attr =>
                             attr.AttributeType == typeof(ParameterInfoAttribute)))
                     .Where(parameterInfo => parameterInfo != null))
        {
            return Convert.ToString(parameterInfo.ConstructorArguments.First().Value) ?? string.Empty;
        }

        return "";
    }
}
