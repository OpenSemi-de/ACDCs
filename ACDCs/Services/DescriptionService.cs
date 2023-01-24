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
        if (parentType.GetInterfaces().Any(i => i == typeof(IElectronicComponent)))
        {
            Type componentType = parentType;
            if (DescriptionTypes.ContainsKey(componentType))
            {
                Type targetNamespaceType = DescriptionTypes[componentType];
                List<Type> targetTypes = targetNamespaceType.Assembly.GetTypes()
                    .Where(type => type.Namespace == targetNamespaceType.Namespace).ToList();
                foreach (Type targetType in targetTypes)
                {
                    var property = targetType.GetProperty(propertyName);
                    if (property != null)
                    {
                        var parameterInfo =
                            property.CustomAttributes.FirstOrDefault(attr => attr.AttributeType == typeof(ParameterInfoAttribute));
                        if (parameterInfo != null)
                        {
                            return Convert.ToString(parameterInfo.ConstructorArguments.First().Value);
                        }
                    }
                }
            }
        }

        return "";
    }
}
